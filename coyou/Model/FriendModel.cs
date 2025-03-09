using Newtonsoft.Json;

namespace coyou;

public class FriendModel
{
    public FriendModel(string? username, DateTime? friendsSince)
    {
        Username = username;
        FriendsSince = friendsSince;
    }

    public FriendModel()
    {
    }

    [JsonProperty("username")] public string? Username { get; set; } 
    [JsonProperty("friendsSince")] public DateTime? FriendsSince { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}