using System.IO;

namespace ViteDotnetCore5.Utils {
    public class FileUtils {

        public static bool Move(string sourcePath, string destPath, string fileName) {
            //LogUtils.WriteLog("sourcePath:" + sourcePath);
            //LogUtils.WriteLog("destPath:" + destPath + "\\" + fileName.Substring(37));
            if (!Directory.Exists(destPath)) {
                Directory.CreateDirectory(destPath);
            }
            
            string path = $"{destPath}\\{fileName.Substring(37)}";
            if (File.Exists(sourcePath)) {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                File.Move(sourcePath, path);
            }

            return true;
        }
    }
}
