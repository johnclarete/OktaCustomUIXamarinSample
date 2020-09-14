using Newtonsoft.Json;

namespace OktaCustomUIXamarinSample.Models
{
    public class TokenResponse
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
