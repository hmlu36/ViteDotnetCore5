using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ViteDotnetCore5.Utils {

    public class LogUtils {


        private static string logDirectory = Directory.GetCurrentDirectory() + "\\log\\";

        /// <summary>
        /// ºg§JLog¿…
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg) {
            try {
                if (!Directory.Exists(logDirectory)) {
                    Directory.CreateDirectory(logDirectory);
                }

                string nowString = DateTime.Now.ToString("yyyyMMdd");
                string logFile = logDirectory + string.Format("log_{0}.txt", nowString);

                File.AppendAllText(logFile, string.Format("{0}: {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg, Environment.NewLine));
            } catch (Exception) {
            }

        }

        internal static void WriteLog(object p) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ºg§JLog¿…
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteLog(Exception exception) {
            try {

                if (!Directory.Exists(logDirectory)) {
                    Directory.CreateDirectory(logDirectory);
                }

                string nowString = DateTime.Now.ToString("yyyyMMdd");
                string logFile = logDirectory + string.Format("log_{0}.txt", nowString);

                File.AppendAllText(logFile, string.Format("{0}: {1}{2}{3}{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine));
            } catch {
            }

        }
    }
}