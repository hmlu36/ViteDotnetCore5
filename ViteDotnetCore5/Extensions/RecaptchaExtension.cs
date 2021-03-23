using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViteDotnetCore5.Models.Auth;
using ViteDotnetCore5.Models.Config;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5.Extensions {

    // Ref: https://github.com/huseyinsimsekk/AspNetCore-ReCAPTCHAv3/blob/master/RecaptchaV3/Extensions/RecaptchaExtension.cs
    // 設定key: https://www.google.com/recaptcha/admin/site/
    public interface IRecaptchaExtension {
        Task<TokenResponseModel> VerifyAsync(string token);
    }

    public class RecaptchaExtension : IRecaptchaExtension {

        private static string GoogleSecretKey = GoogleRecaptcha.Instance.SecretKey ?? "";
        private static string GoogleRecaptchaVerifyApi = GoogleRecaptcha.Instance.VefiyAPIAddress ?? "";
      
        public async Task<TokenResponseModel> VerifyAsync(string token) {
            if (string.IsNullOrEmpty(token)) {
                throw new Exception("Token cannot be null!");
            }
            using (var client = new HttpClient()) {
                var response = await client.GetStringAsync($"{GoogleRecaptchaVerifyApi}?secret={GoogleSecretKey}&response={token}");
                LogUtils.WriteLog(JsonConvert.SerializeObject(response));
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(response);
                return tokenResponse;
            }
        }
    }
}
