using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Model
{
    public class TokenRefreshModel(string? refreshToken)
    {
        [JsonProperty("client_id")] public string? ClientId { get; } = Constants.Google.ClientId;
        [JsonProperty("refresh_token")] public string? RefreshToken { get; set; } = refreshToken;
        [JsonProperty("grant_type")] public string? GrantType { get; } = "refresh_token";

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
