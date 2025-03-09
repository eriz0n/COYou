using Newtonsoft.Json;

namespace coyou;

public class LeaderboardModel
{
    public LeaderboardModel()
    {
    }

    public LeaderboardModel(string? username, double? totalEmissions)
    {
        Username = username;
        TotalEmissions = totalEmissions;
    }

    [JsonProperty("username")] public string? Username { get; set; }
    [JsonProperty("totalEmissions")] public double? TotalEmissions { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}