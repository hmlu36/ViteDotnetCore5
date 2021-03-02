
using System;
using System.ComponentModel;
using System.Reflection;

namespace ViteDotnetCore5.Extensions {
     // 參考: https://marcus116.blogspot.com/2018/12/how-to-get-enum-description-attribute.html
    public static class EnumExtensions {
        // 取得 Enum 列舉 Attribute Description 設定值
        public static string GetDescription(this Enum source) {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return source.ToString();
        }

        // 參考: https://stackoverflow.com/questions/16100/convert-a-string-to-an-enum-in-c-sharp
        public static T ToEnum<T>(this string value, bool ignoreCase = true) {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        
        public static T ToEnum<T>(this int value, bool ignoreCase = true) {
            return (T)Enum.Parse(typeof(T), value.ToString(), ignoreCase);
        }
    }
}
