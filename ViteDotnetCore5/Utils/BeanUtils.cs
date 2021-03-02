using System.Collections.Generic;
using System.Linq;

namespace ViteDotnetCore5.Utils {
    public static class BeanUtils {

        private static List<string> idFiled = new List<string> { "Id", "SeqNo", "CreatedUser", "CreatedAt", "UpdatedUser", "UpdatedAt" }; // 排除id跟seq欄位

        public static void CopyProperties(object src, object dest) {
            CopyProperties(src, dest, null);
        }

        public static void CopyProperties(object src, object dest, List<string> excludeFields) {
            foreach (var source in src.GetType().GetProperties().Where(p => p.CanRead &&
                                                                            !idFiled.Contains(p.Name) &&
                                                                            (excludeFields == null || !excludeFields.Contains(p.Name)))) {
                var prop = dest.GetType().GetProperty(source.Name);
                if (prop?.CanWrite == true) {
                    prop.SetValue(dest, source.GetValue(src));
                }
            }
        }
    }
}
