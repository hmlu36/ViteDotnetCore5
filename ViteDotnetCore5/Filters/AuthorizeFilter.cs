
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ViteDotnetCore5.Extensions;
using ViteDotnetCore5.Models.Enums;
using ViteDotnetCore5.Services;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Filters {

    // 參考 https://stackoverflow.com/questions/55102026/custom-authorization-filter-in-net-core-api
    //      https://stackoverflow.com/questions/38340078/how-to-decode-jwt-token
    // config DI https://www.cnblogs.com/OpenCoder/p/9760777.html
    public class AuthorizeFilterAttribute : TypeFilterAttribute {
        public AuthorizeFilterAttribute(params RoleEnums[] permissions) : base(typeof(AuthorizeFilter)) {
            Arguments = new object[] { permissions };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter {
        private readonly RoleEnums[] _permissions;

        public AuthorizeFilter(RoleEnums[] permissions = null) {
            _permissions = permissions;
            //LogUtils.WriteLog("Roles:" + JsonConvert.SerializeObject(_permissions));
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (HasAllowAnonymous(context)) {
                return;
            }

            IUserTokenService userTokenService = context.HttpContext.RequestServices.GetRequiredService<IUserTokenService>();

            var request = context.HttpContext.Request;
            bool success = false;
            // 判斷是否有 JWT Token
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            /*
            LogUtils.WriteLog("token:" + token);
            JwtUtils.ValidateJwtToken(token);
            LogUtils.WriteLog("success:" + success);
            */
            var userToken = userTokenService.FindUserToken(context.GetAccessToken());
            LogUtils.WriteLog("userToken:" + JsonConvert.SerializeObject(userToken));
            LogUtils.WriteLog("token:" + token);
            if (token == "null" || !JwtUtils.ValidateJwtToken(token) || userToken == null || DateTime.Now > userToken.ExpireAt) {
                context.Result = new UnauthorizedResult();
            }
        }

        /*
        private bool CheckUserPermission(AuthorizationFilterContext context) {
            if (_permissions.Any()) { 
                var loginUser = RoleUtils.GetLoginUser(context.GetLoginUserId());
                return _permissions.Contains(loginUser.Role);
            }
            return true;
        }
        */

        // allowanonymous filter
        // 參考: https://stackoverflow.com/questions/59310193/allowanonymous-is-not-ignored-in-asp-net-core-custom-authenticationhandler-after
        private static bool HasAllowAnonymous(AuthorizationFilterContext context) {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++) {
                if (filters[i] is IAllowAnonymousFilter) {
                    return true;
                }
            }

            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null) {
                return true;
            }

            return false;
        }
    }
}
