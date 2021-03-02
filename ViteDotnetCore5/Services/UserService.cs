using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViteDotnetCore5.Models.Auth;
using ViteDotnetCore5.Models.Criteria;
using ViteDotnetCore5.Models.EFCore;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Services {
    public interface IUserService : IGenericService<User, UserCriteria> {


        bool IsValid(LoginUser form, out User dbUser);
    }

    public class UserService : GenericService<User, UserCriteria>, IUserService {

        public UserService(GeoEPPContext dbContext, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(dbContext, httpContextAccessor, env) {
            
        }

        public bool IsValid(LoginUser form, out User dbUser) {
            dbUser = dbContext.Users.Where(u => u.Account.Equals(form.Account)).SingleOrDefault();
            return dbUser == null ? false : PasswordUtils.Validate(form.Password, dbUser.Password);
        }


        protected override void CompositeCriteria(UserCriteria criteria) {
            /*
            if (!string.IsNullOrEmpty(criteria.Role)) {
                predicate = predicate.And(u => u.Auz == criteria.Role);
            }

            if (!string.IsNullOrEmpty(criteria.QueryParameter)) {
                predicate = predicate.And(u => EF.Functions.Like(u.Account, $"%{criteria.QueryParameter}%") || EF.Functions.Like(u.Idname, $"%{criteria.QueryParameter}%"));
            }

            string loginUserRole = httpContextAccessor.GetLoginUserRole();
            Guid loginuserId = httpContextAccessor.GetLoginUserId();
            if (!string.IsNullOrEmpty(loginUserRole) && "使用者".Equals(loginUserRole)) {
                predicate = predicate.And(u => u.Id == loginuserId);
            }
            */
        }


        protected override void PreProcess(User user) {
            if (!string.IsNullOrEmpty(user.Password) && user.Password.Length != 44) {
                user.Password = PasswordUtils.Hash(user.Password);
            }
        }


        protected override void PostProcess(User user) {
            if (user.Id != Guid.Empty && string.IsNullOrEmpty(user.Password)) {
                dbContext.Entry(user).Property("Password").IsModified = false;
            }
        }
    }
}
