
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ViteDotnetCore5.Models.Auth;
using ViteDotnetCore5.Models.Config;
using ViteDotnetCore5.Models.EFCore;

namespace ViteDotnetCore5.Services {
    public interface IAuthService {
        bool IsAuthenticated(LoginUser form, out object token);

        void Logout();
    }

    public class AuthService : IAuthService {
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;

        public AuthService(IUserService userService, IUserTokenService userTokenService) {
            _userService = userService;
            _userTokenService = userTokenService;
        }

        public bool IsAuthenticated(LoginUser form, out object token) {
            User dbUser = null;
            token = null;

            if (!_userService.IsValid(form, out dbUser)) {
                return false;
            }

            token = _userTokenService.GenerateJwtToken(dbUser);
            return true;
        }



        public void Logout() {
        }

        /*
        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using(var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        */

    }
}
