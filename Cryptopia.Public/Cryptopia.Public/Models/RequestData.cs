using Newtonsoft.Json;

namespace Cryptopia.Public.Models {
    public class RequestData {
        [JsonProperty("Success")]
        public string Success { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
