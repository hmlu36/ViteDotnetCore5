using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ViteDotnetCore5.Extensions;
using ViteDotnetCore5.Models.Config;
using ViteDotnetCore5.Models.EFCore;
using ViteDotnetCore5.Repositories;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Services {

    public interface IUserTokenService : IGenericRepository<UserToken> {

        object GenerateJwtToken(User dbUser);

        object RefreshToken(Guid refreshToken);

        UserToken FindUserToken(string accessToken);


        void DeleteJwtToken();
    }

    public class UserTokenService : GenericRepository<UserToken>, IUserTokenService {
        private readonly GeoEPPContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public UserTokenService(GeoEPPContext dbContext, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor) {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        public object GenerateJwtToken(User dbUser) {
            var expires = DateTime.Now.AddMinutes(JwtSettings.Instance.AccessExpiration);
            var refreshToken = Guid.NewGuid();
            var claims = new List<Claim> { new Claim("Name", dbUser.Name),
                                           new Claim("Id", dbUser.Id.ToString()),
                                           new Claim("Role", string.Join(",", dbUser.UserRoles.Select(ur => ur.Role).Select(r => r.Name))),
                                           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Instance.Secret));

            // 建立 SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor {
                Issuer = JwtSettings.Instance.Issuer,
                //Audience = issuer, // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
                //NotBefore = DateTime.Now, // 預設值就是 DateTime.Now
                //IssuedAt = DateTime.Now, // 預設值就是 DateTime.Now
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // 寫入資料庫
            var accessToken = tokenHandler.WriteToken(securityToken);
            UserToken userToken = new UserToken() {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Ipaddress = httpContextAccessor.GetIPAddress(),
                ExpireAt = expires,
                UserId = dbUser.Id
            };

            Create(userToken);

            // 刪除過期Token
            var expireUserTokens = dbContext.UserTokens.Where(u => u.UserId == dbUser.Id && u.ExpireAt < DateTime.Now).ToList();
            Delete(expireUserTokens);

            return new { AccessToken = accessToken, RefreshToken = refreshToken };
        }



        public object RefreshToken(Guid refreshToken) {
            if (refreshToken != Guid.Empty) {
                //需查詢資料庫
                var dbToken = dbContext.UserTokens.Include(ut => ut.User).SingleOrDefault(u => u.RefreshToken == refreshToken);
                //產生一組新的 Token 和 Refresh Token
                return GenerateJwtToken(dbToken.User);
            }
            return null;
        }


        public UserToken FindUserToken(string accessToken) {
            LogUtils.WriteLog("accessToken:" + accessToken);
            return string.IsNullOrEmpty(accessToken) ? null : dbContext.UserTokens.SingleOrDefault(ut => ut.AccessToken == accessToken);
        }

        public void DeleteJwtToken() {
            var jwtToken = httpContextAccessor.GetJwtToken();
            //LogUtils.WriteLog(JsonConvert.SerializeObject(jwtToken));
            if (jwtToken.Claims.Any()) {
                var userId = JwtUtils.GetJwtTokenValue(jwtToken, "Id");
                var accessToken = httpContextAccessor.GetAccessToken();
                //LogUtils.WriteLog("userId:" + userId + ", jti:" + jti);
                var dbUserTokens = dbContext.UserTokens.Where(u => u.UserId == Guid.Parse(userId) && u.AccessToken == accessToken).ToList();
                //_memoryCache.Remove(jti);
                Delete(dbUserTokens);
            }
        }

    }
}
