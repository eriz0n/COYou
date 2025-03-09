using Newtonsoft.Json;

namespace coyou;

public class FriendRequestModel
{
    public FriendRequestModel()
    {
    }

    public FriendRequestModel(string? user, DateTime? sentOn)
    {
        User = user;
        SentOn = sentOn;
    }

    [JsonProperty("user")] public string? User { get; set; }
    [JsonProperty("sentOn")] public DateTime? SentOn { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}