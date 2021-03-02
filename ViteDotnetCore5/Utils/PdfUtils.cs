using Microsoft.AspNetCore.Mvc;
using OpenHtmlToPdf;
using System;
using System.IO;

namespace ViteDotnetCore5.Utils {
    public class PdfUtils {

        public static FileContentResult ExportPdf(string html, string fileName) {
            // 需開啟此路徑底下的權限, OpenHtmlToPdf轉檔時會借用此路徑產生PDF, 沒有開啟會出現Access Denied
            // 參考: https://stackoverflow.com/questions/58579634/openhtmltopdf-access-is-denied-with-asp-net
            //LogUtils.WriteLog(Path.GetTempPath();
            var pdf = Pdf.From(html)
                         .OfSize(PaperSize.A4)
                         .Content();

            using (var net = new System.Net.WebClient()) {
                return new FileContentResult(pdf, "application/pdf") {
                    FileDownloadName = $"{DateTime.Now:yyyy-MM-dd_HH:mm:ss}{fileName}.pdf"
                };
            }
        }
    }
}
