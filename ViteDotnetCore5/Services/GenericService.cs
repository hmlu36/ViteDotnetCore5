using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViteDotnetCore5.Models.EFCore;
using ViteDotnetCore5.Repositories;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Services {
   public interface IGenericService<T, C> : IGenericRepository<T> where T : class where C : class {
        IEnumerable<T> FindAll(C criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        FileContentResult Download(C criteria);
    }


    public class GenericService<T, C> : GenericRepository<T> where T : class where C : class {

        protected readonly GeoEPPContext dbContext;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly string uploadFolder;
        protected ExpressionStarter<T> predicate = PredicateBuilder.New<T>(true);
        protected StringBuilder html = null;
        protected string fileName = null;

        public GenericService(GeoEPPContext dbContext, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(dbContext, httpContextAccessor) {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            uploadFolder = $"{env.WebRootPath}\\Upload";
        }

        public virtual IEnumerable<T> FindAll(C criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) {
            CompositeCriteria(criteria);
            var query = _dbContext.Set<T>().AsQueryable();
            if (include != null) {
                query = include(query);
            }

            return query.Where(predicate)
                        .OrderByDescending(o => EF.Property<DateTime>(o, "UpdatedAt"));
        }

        // 組成查詢條件
        protected virtual void CompositeCriteria(C criteria) {

        }



        protected virtual void ComposeHtmlContent(C criteria) {

        }

        // 檔案下載
        public FileContentResult Download(C criteria) {
            ComposeHtmlContent(criteria);
            return PdfUtils.ExportPdf(html.ToString(), fileName);
        }

    }
}
