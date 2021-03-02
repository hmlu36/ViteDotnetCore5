

using Newtonsoft.Json;

namespace ViteDotnetCore5.Models.Result {
    public class ResultModel {
        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public ResultModel() {
            Success = true;
        }

        public ResultModel(string errors) {
            Success = false;
            Message = errors;
        }
    }
}
