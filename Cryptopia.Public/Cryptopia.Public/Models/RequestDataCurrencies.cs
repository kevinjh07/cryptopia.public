using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cryptopia.Public.Models
{
    public class RequestDataCurrencies : RequestData
    {
        [JsonProperty("Data")]
        public List<Coin> Data { get; set; }
    }
}
