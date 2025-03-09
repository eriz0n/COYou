using Newtonsoft.Json;

namespace coyou;

public class EnergyConsumptionModel
{
    public EnergyConsumptionModel()
    {
    }
    public EnergyConsumptionModel(string? type, double? emissionsPerKwh)
    {
        Type = type;
        EmissionsPerKwh = emissionsPerKwh;
    }

 

    [JsonProperty("type")] public string? Type { get; set; } 
    [JsonProperty("emissionsPerKwh")] public double? EmissionsPerKwh { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}