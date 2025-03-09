using Newtonsoft.Json;

namespace coyou;

public class EmissionsModel
{
    public EmissionsModel()
    {
    }

    public EmissionsModel(double? trafficEmissions, string? diet, Dictionary<string, double>? waste,
        Dictionary<string, double>? energy)
    {
        TrafficEmissions = trafficEmissions;
        Diet = diet;
        Waste = waste;
        Energy = energy;
    }

    [JsonProperty("trafficEmissions")] public double? TrafficEmissions { get; set; }
    [JsonProperty("diet")] public string? Diet { get; set; }
    [JsonProperty("waste")] public Dictionary<string, double>? Waste { get; set; }
    [JsonProperty("energy")] public Dictionary<string, double>? Energy { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}