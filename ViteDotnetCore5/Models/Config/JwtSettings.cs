using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViteDotnetCore5.Models.Config {

    // Setting 採用singleton pattern
    // 參考: https://stackoverflow.com/questions/45885615/asp-net-core-access-configuration-from-static-class

    public class JwtSettings {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessExpiration { get; set; }

        // enable set in singleton property instance to work correctly.
        public static JwtSettings Instance { get; set; } = new JwtSettings();
    }
}
