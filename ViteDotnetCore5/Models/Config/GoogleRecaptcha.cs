using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViteDotnetCore5.Models.Config {
    public class GoogleRecaptcha {
        public string VefiyAPIAddress { set; get; }
        public string SiteKey { set; get; }
        public string SecretKey { set; get; }
        public string RecaptchaThreshold { set; get; }

        public static GoogleRecaptcha Instance { get; set; } = new GoogleRecaptcha();
    }
}
