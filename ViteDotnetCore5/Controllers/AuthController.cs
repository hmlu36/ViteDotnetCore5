using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViteDotnetCore5.Extensions;
using ViteDotnetCore5.Models;
using ViteDotnetCore5.Models.Auth;
using ViteDotnetCore5.Models.Result;
using ViteDotnetCore5.Services;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthService authService;
        private readonly IUserTokenService userTokenService;
        private IRecaptchaExtension recaptcha;
        public AuthController(IAuthService authService, IUserTokenService userTokenService, IRecaptchaExtension recaptcha) {
            this.authService = authService;
            this.userTokenService = userTokenService;
            this.recaptcha = recaptcha;
        }

        /// <summary>
        /// google reCAPTCHA V3
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        // 呼叫時會因為網址過長IIS(超過320個字元), IIS視為無效網址
        // 解法 => 改成querystring就可以
        // 範例: http://path.to.website/very-long-string-goes-here/ => http://path.to.website/?key=very-long-string-goes-here
        // Ref: https://stackoverflow.com/questions/40448588/i-get-a-400-bad-request-invalid-url-when-the-length-of-the-request-exceeds-320
        [HttpPost("Verify")]
        public async Task<ActionResult> Verify([FromQuery(Name = "token")] string token) {
            var verified = await recaptcha.VerifyAsync(token);
            return Ok(verified);
        }

        /// <summary>
        /// 登入驗證，驗證成功會回傳JWT Token
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginUser form) {
            if (!ModelState.IsValid) {
                return Ok(form);
            }

            var result = new ResultModel();
            try {
                object token = "";

                if (authService.IsAuthenticated(form, out token)) {
                    result.Data = token;
                } else {
                    result.Success = false;
                    result.Message = "登入失敗";
                }
            } catch (Exception e) {
                result.Success = false;
                result.Message = e.StackTrace + Environment.NewLine + e.Message;
            }
            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken([FromQuery(Name = "refreshToken")] Guid refreshToken) {

            //LogUtils.WriteLog("refreshToken:" + refreshToken);
            var result = new ResultModel();
            try {
                result.Data = userTokenService.RefreshToken(refreshToken);
            } catch (Exception e) {
                result.Success = false;
                result.Message = e.StackTrace + Environment.NewLine + e.Message;
            }
            return Ok(result);
        }

        /// <summary>
        /// 登出，抓JWT Token刪除登入資訊
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public ActionResult Logout() {
            var result = new ResultModel();
            try {
                authService.Logout();
            } catch (Exception e) {
                result.Success = false;
                result.Message = e.StackTrace + Environment.NewLine + e.Message;
            }
            return Ok(result);
        }
    }
}
