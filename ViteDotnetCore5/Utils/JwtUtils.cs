using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using ViteDotnetCore5.Models.Config;

namespace ViteDotnetCore5.Utils {
    public class JwtUtils {

        public static string GetJwtTokenValue(JwtSecurityToken token, string key) {
            return token.Claims.Any() ? token.Claims.First(claim => claim.Type == key)?.Value : null;
        }


        public static bool ValidateJwtToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.Instance.Secret);
            try {
                LogUtils.WriteLog("validateJwtToken: " + token);
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return validatedToken != null;
            } catch (Exception ex) {
                LogUtils.WriteLog(ex.Message + ex.StackTrace);
                // return null if validation fails
                return false;
            }
        }

    }
}
