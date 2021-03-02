using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GeoUVP.Utils {
    public class ValidateUtils {

        public static bool ContainUuid(string input) {
            if (!string.IsNullOrEmpty(input) && input.Length > 36) {
                return new Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$").Match(input.Substring(0, 36)).Success;
            }
            return false;
        }

        public static bool Phone(string input) {
            if (!string.IsNullOrEmpty(input)) {
                return new Regex("^09[0-9]{8}$").Match(input).Success;
            }
            return true;
        }

        public static bool Tel(string input) {
            if (!string.IsNullOrEmpty(input)) {
                return new Regex("\\d{2,4}-?\\d{3,4}-?\\d{3,4}#?(\\d+)?").Match(input).Success;
            }
            return true;
        }

        // 密碼長度至少6碼，要有大小寫、數字、特殊字元
        public static bool Password(string input) {
            return !string.IsNullOrEmpty(input) && new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[~#?!@$%^&*-+]).{6,}$").Match(input).Success;
        }

        // 身分證驗證 參考 https://ithelp.ithome.com.tw/articles/10202558
        public static bool IdNumber(string idNumber) {
            if (string.IsNullOrEmpty(idNumber)) {
                return true;
            }

            // 使用「正規表達式」檢驗格式 [A~Z] {1}個數字 [0~9] {9}個數字
            var regex = new Regex("^[A-Z]{1}[0-9]{9}$");
            if (!regex.IsMatch(idNumber)) {
                //Regular Expression 驗證失敗，回傳 ID 錯誤
                return false;
            }

            //除了檢查碼外每個數字的存放空間 
            int[] seed = new int[10];

            //建立字母陣列(A~Z)
            //A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
            //P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35            
            string[] charMapping = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };
            string target = idNumber.Substring(0, 1); //取第一個英文數字
            for (int index = 0; index < charMapping.Length; index++) {
                if (charMapping[index] == target) {
                    index += 10;
                    //10進制的高位元放入存放空間   (權重*1)
                    seed[0] = index / 10;

                    //10進制的低位元*9後放入存放空間 (權重*9)
                    seed[1] = (index % 10) * 9;

                    break;
                }
            }
            for (int index = 2; index < 10; index++) //(權重*8~1)
            {   //將剩餘數字乘上權數後放入存放空間                
                seed[index] = Convert.ToInt32(idNumber.Substring(index - 1, 1)) * (10 - index);
            }
            //檢查是否符合檢查規則，10減存放空間所有數字和除以10的餘數的個位數字是否等於檢查碼            
            //(10 - ((seed[0] + .... + seed[9]) % 10)) % 10 == 身分證字號的最後一碼   
            if ((10 - (seed.Sum() % 10)) % 10 != Convert.ToInt32(idNumber.Substring(9, 1))) {
                return false;
            }

            return true;
        }

        // 參考 https://dotblogs.com.tw/ChentingW/2020/03/29/000036
        public static bool TaxIdnumber(string taxIdnumber) {
            if (string.IsNullOrEmpty(taxIdnumber)) {
                return true;
            }
            Regex regex = new Regex(@"^\d{8}$");
            Match match = regex.Match(taxIdnumber);
            if (!match.Success) {
                return false;
            }
            int[] idNoArray = taxIdnumber.ToCharArray().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            int[] weight = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };

            int subSum;     //小和
            int sum = 0;    //總和
            int sumFor7 = 1;
            for (int i = 0; i < idNoArray.Length; i++) {
                subSum = idNoArray[i] * weight[i];
                sum += (subSum / 10)   //商數
                     + (subSum % 10);  //餘數                
            }
            if (idNoArray[6] == 7) {
                //若第7碼=7，則會出現兩種數值都算對，因此要特別處理。
                sumFor7 = sum + 1;
            }
            return (sum % 10 == 0) || (sumFor7 % 10 == 0);
        }

        // 參考 https://gist.github.com/yyc1217/3856443
        public static bool Passport(string input) {
            if (string.IsNullOrEmpty(input)) {
                return true;
            }

            char[] pidCharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            // 原居留證第一碼英文字應轉換為10~33，十位數*1，個位數*9，這裡直接作[(十位數*1) mod 10] + [(個位數*9) mod 10]
            int[] pidResidentFirstInt = { 1, 10, 9, 8, 7, 6, 5, 4, 9, 3, 2, 2, 11, 10, 8, 9, 8, 7, 6, 5, 4, 3, 11, 3, 12, 10 };

            // 原居留證第二碼英文字應轉換為10~33，並僅取個位數*8，這裡直接取[(個位數*8) mod 10]
            int[] pidResidentSecondInt = { 0, 8, 6, 4, 2, 0, 8, 6, 2, 4, 2, 0, 8, 6, 0, 4, 2, 0, 8, 6, 4, 2, 6, 0, 8, 4 };
            input = input.ToUpper();// 轉換大寫
            char[] strArr = input.ToCharArray();// 字串轉成char陣列
            int verifyNum = 0;
            if (new Regex("[A-Z]{1}[A-D]{1}[0-9]{8}").Match(input).Success) {
                // 第一碼
                verifyNum += pidResidentFirstInt[Array.IndexOf(pidCharArray, strArr[0])];
                // 第二碼
                verifyNum += pidResidentSecondInt[Array.IndexOf(pidCharArray, strArr[1])];
                // 第三~八碼
                for (int i = 2, j = 7; i < 9; i++, j--) {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;

                return verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
            }
            return false;
        }

        // 居留證驗證
        // 參考 https://dotblogs.com.tw/ChentingW/2020/03/29/001426
        public bool CheckResidentID(string idNo) {
            if (idNo == null) {
                return false;
            }
            idNo = idNo.ToUpper();
            Regex regex = new Regex(@"^([A-Z])(A|B|C|D|8|9)(\d{8})$");
            Match match = regex.Match(idNo);
            if (!match.Success) {
                return false;
            }

            if ("ABCD".IndexOf(match.Groups[2].Value) >= 0) {
                //舊式
                return CheckOldResidentID(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            } else {
                //新式(2021/01/02)正式生效
                return CheckNewResidentID(match.Groups[1].Value, match.Groups[2].Value + match.Groups[3].Value);
            }
        }

        // 舊式檢核
        private bool CheckOldResidentID(string firstLetter, string secondLetter, string num) {
            //建立字母對應表(A~Z)
            //A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
            //P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32 Z=33 I=34 O=35 
            string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
            string transferIdNo =
                $"{alphabet.IndexOf(firstLetter) + 10}" +
                $"{(alphabet.IndexOf(secondLetter) + 10) % 10}" +
                $"{num}";
            int[] idNoArray = transferIdNo.ToCharArray()
                                          .Select(c => Convert.ToInt32(c.ToString()))
                                          .ToArray();

            int sum = idNoArray[0];
            int[] weight = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
            for (int i = 0; i < weight.Length; i++) {
                sum += weight[i] * idNoArray[i + 1];
            }
            return (sum % 10 == 0);
        }

        // 新式檢核
        private bool CheckNewResidentID(string firstLetter, string num) {
            //建立字母對應表(A~Z)
            //A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
            //P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32 Z=33 I=34 O=35 
            string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
            string transferIdNo = $"{(alphabet.IndexOf(firstLetter) + 10)}" +
                                  $"{num}";
            int[] idNoArray = transferIdNo.ToCharArray()
                                          .Select(c => Convert.ToInt32(c.ToString()))
                                          .ToArray();

            int sum = idNoArray[0];
            int[] weight = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
            for (int i = 0; i < weight.Length; i++) {
                sum += (weight[i] * idNoArray[i + 1]) % 10;
            }
            return (sum % 10 == 0);
        }
    }
}
