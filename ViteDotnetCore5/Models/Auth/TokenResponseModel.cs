using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViteDotnetCore5.Models.Auth {
    public class TokenResponseModel {
        public bool Success { get; set; }

        public decimal Score { get; set; }

        public string Action { get; set; }
        
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
