using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ViteDotnetCore5.Models.Config;

namespace ViteDotnetCore5.Utils {
    public class TokenSecurity {

        public static JwtSecurityToken GenerateJwt(string email) {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, email)
              };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Instance.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
              JwtSettings.Instance.Issuer,
              JwtSettings.Instance.Audience,
              claims,
              expires: DateTime.Now.AddMinutes(20),
              signingCredentials: creds);

            return token;

        }
    }
}
