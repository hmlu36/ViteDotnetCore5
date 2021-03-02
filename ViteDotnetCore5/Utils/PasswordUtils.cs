using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ViteDotnetCore5.Utils {

    // 參考 https://docs.microsoft.com/zh-tw/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
    // 參考 https://www.c-sharpcorner.com/article/hashing-in-asp-net-core-2-0/
    public class PasswordUtils {

        public static string Hash(string password) {

            // generate a 128-bit salt using a secure PRNG   
            string salt;
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create()) {
                salt = Convert.ToBase64String(randomBytes);
            }
            // derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                               salt: Encoding.UTF8.GetBytes(salt),
                                                               prf: KeyDerivationPrf.HMACSHA256,
                                                               iterationCount: 10000,
                                                               numBytesRequested: 256 / 8));
        }

        public static bool Validate(string value, string hash) => Hash(value) == hash;

        // 參考 https://ithelp.ithome.com.tw/articles/10206420
        public static string GeneratePassword() {
            //密碼亂數
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*_+";
            int passwordLength = 10;//密碼長度
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++) {
                //allowedChars -> 這個String ，隨機取得一個字，丟給chars[i]
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
