using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ViteDotnetCore5.Extensions;

namespace ViteDotnetCore5.Repositories {
    // 參考 https://codebrains.io/asp-net-core-entity-framework-repository-pattern/
    public interface IGenericRepository<T> where T : class {
        IEnumerable<T> FindAll();

        IEnumerable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        T FindById(Guid id);

        T FindById(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        void Create(T entity);

        void Create(List<T> entities);

        void Update(T entity);

        void Update(List<T> entities);

        void Delete(Guid id);

        void Delete(List<T> entities);

        void Invalid(Guid id);
    }


    public class GenericRepository<T> : IGenericRepository<T> where T : class {

        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DbContext _dbContext;

        public GenericRepository(DbContext dbContext, IHttpContextAccessor httpContextAccessor) {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // 取得有效欄位，參考 https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.ef.property?view=efcore-3.1
        public virtual IEnumerable<T> FindAll() {
            return _dbContext.Set<T>();
        }


        // 參考 https://stackoverflow.com/questions/48671263/generic-repository-includes-and-filtering
        // include參考 https://stackoverflow.com/questions/50137839/theninclude-for-sub-entity-in-entity-framework-core-2
        public virtual IEnumerable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) {
            var query = _dbContext.Set<T>().AsQueryable();
            if (include != null) {
                query = include(query);
            }

            if (filter != null) {
                query = query.Where(filter);
            }

            if (orderBy != null) {
                query = orderBy(query);
            }

            return query;
        }


        public virtual T FindById(Guid id) {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual T FindById(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) {
            var query = _dbContext.Set<T>().AsQueryable();
            if (include != null) {
                query = include(query);
            }

            return query.FirstOrDefault(e => EF.Property<Guid>(e, "Id") == id);
        }

        // 處理上傳檔案
        protected virtual void PreProcess(T entity) {

        }

        protected virtual void PostProcess(T entity) {

        }


        public virtual void Create(T entity) {
            PreProcess(entity);
            InitCreatedUpdateUser(entity);
            PostProcess(entity);
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Create(List<T> entities) {
            foreach (var entity in entities) {
                PreProcess(entity);
                InitCreatedUpdateUser(entity);
                PostProcess(entity);
            }

            _dbContext.Set<T>().AddRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual void Update(T entity) {
            PreProcess(entity);
            InitUpdateUser(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            SetScaffoldUnModified(entity);
            PostProcess(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Update(List<T> entities) {
            foreach (var entity in entities) {
                PreProcess(entity);
                InitUpdateUser(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                SetScaffoldUnModified(entity);
                PostProcess(entity);
            }
            _dbContext.SaveChanges();
        }


        private void SetScaffoldUnModified(T entity) {
            if (typeof(T).GetProperties().Any(p => p.Name == "SeqNo")) {
                _dbContext.Entry(entity).Property("SeqNo").IsModified = false; // 忽略SeqNo欄位(db流水號，自動取號不能更新)
            }

            if (typeof(T).GetProperties().Any(p => p.Name == "CreatedUser")) {
                _dbContext.Entry(entity).Property("CreatedUser").IsModified = false;
            }

            if (typeof(T).GetProperties().Any(p => p.Name == "CreatedAt")) {
                _dbContext.Entry(entity).Property("CreatedAt").IsModified = false;
            }
        }

        public virtual void Delete(Guid id) {
            var dbEntity = FindById(id);
            _dbContext.Set<T>().Remove(dbEntity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(List<T> entities) {
            foreach (var entity in entities) {
                _dbContext.Set<T>().Remove(entity);
            }
            _dbContext.SaveChanges();
        }

        public virtual void Invalid(Guid id) {
            var dbEntity = FindById(id);
            InitUpdateUser(dbEntity);
            TrySetProperty(dbEntity, "Status", 0m);
            _dbContext.Entry(dbEntity).Property("Status").IsModified = true;
            _dbContext.Entry(dbEntity).Property("UpdatedUser").IsModified = true;
            _dbContext.Entry(dbEntity).Property("UpdatedAt").IsModified = true;
            _dbContext.SaveChanges();
        }

        // generic entity set property
        // 參考: https://stackoverflow.com/questions/16962727/how-to-set-properties-on-a-generic-entity
        private void TrySetProperty(object obj, string property, object value) {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite) {
                prop.SetValue(obj, value, null);
            }
        }


        private void InitCreatedUpdateUser(T entity) {
            TrySetProperty(entity, "CreatedAt", DateTime.Now);
            TrySetProperty(entity, "CreatedUser", _httpContextAccessor.GetLoginUserId());
            InitUpdateUser(entity);
        }

        private void InitUpdateUser(T entity) {
            TrySetProperty(entity, "UpdatedAt", DateTime.Now);
            TrySetProperty(entity, "UpdatedUser", _httpContextAccessor.GetLoginUserId());
        }

    }
}
