using Newtonsoft.Json;

namespace coyou;

public class WasteModel
{
    public WasteModel()
    {
    }
    public WasteModel(string? type, double? emissionsPerKg)
    {
        Type = type;
        EmissionsPerKg = emissionsPerKg;
    }



    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("emissionsPerKg")] public double? EmissionsPerKg { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}