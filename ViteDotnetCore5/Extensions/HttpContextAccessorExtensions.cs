
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Extensions {
    public static class HttpContextAccessorExtensions {

        // 取得登入資料
        public static Guid? GetLoginUserId(this IHttpContextAccessor httpContextAccessor) {
            var id = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(id) ? Guid.Parse(id) : null;
        }

        public static string GetLoginUserRole(this IHttpContextAccessor httpContextAccessor) {
            return httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static string GetIPAddress(this IHttpContextAccessor httpContextAccessor) {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string GetAccessToken(this IHttpContextAccessor httpContextAccessor) {
            string jwtString = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(jwtString) && jwtString.StartsWith("Bearer")) {
                return jwtString.Replace("Bearer", "").Trim();
            }
            return null;
        }

        public static JwtSecurityToken GetJwtToken(this IHttpContextAccessor httpContextAccessor) {
            string jwtString = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(jwtString) && jwtString.StartsWith("Bearer")) {
                return new JwtSecurityTokenHandler().ReadToken(jwtString.Replace("Bearer", "").Trim()) as JwtSecurityToken;
            }
            return new JwtSecurityToken();
        }

        public static Guid? GetRefreshToken(this IHttpContextAccessor httpContextAccessor) {
            var id = httpContextAccessor.HttpContext.User?.FindFirst(c => c.Type == "RefreshToken")?.Value;
            return !string.IsNullOrEmpty(id) ? Guid.Parse(id) : null;
        }


        public static JwtSecurityToken GetJwtToken(this AuthorizationFilterContext context) {
            string jwtString = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(jwtString) && jwtString.StartsWith("Bearer") && jwtString.Replace("Bearer", "").Trim() != "null") {
                return new JwtSecurityTokenHandler().ReadToken(jwtString.Replace("Bearer", "").Trim()) as JwtSecurityToken;
            }

            return new JwtSecurityToken();
        }

        public static Guid GetLoginUserId(this AuthorizationFilterContext context) {
            var jwtToken = GetJwtToken(context);
            var id = JwtUtils.GetJwtTokenValue(jwtToken, "Id");
            return (jwtToken != null && !string.IsNullOrEmpty(id)) ? Guid.Parse(id) : new Guid();
        }

        public static string GetAccessToken(this AuthorizationFilterContext context) {
            string jwtString = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(jwtString) && jwtString.StartsWith("Bearer") && jwtString.Replace("Bearer", "").Trim() != "null") {
                return jwtString.Replace("Bearer", "").Trim();
            }
            return null;
        }

    }
}
