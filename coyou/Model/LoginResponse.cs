using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coyou;

public class LoginResponse
{
    public LoginResponse()
    {
    }

    public LoginResponse(string? accessToken, string? idToken, int? expiresIn, string? refreshToken, string? scope, string? tokenType)
    {
        AccessToken = accessToken;
        IdToken = idToken;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
        Scope = scope;
        TokenType = tokenType;
    }

    [JsonProperty("access_token")] public string? AccessToken { get; set; }
    [JsonProperty("id_token")] public string? IdToken { get; set; }
    [JsonProperty("expires_in")] public int? ExpiresIn { get; set; } 
    [JsonProperty("refresh_token")] public string? RefreshToken { get; set; }
    [JsonProperty("scope")] public string? Scope { get; set; }
    [JsonProperty("token_type")] public string? TokenType { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}