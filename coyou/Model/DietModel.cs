using Newtonsoft.Json;

namespace coyou;

public class DietModel
{
    public DietModel()
    {
    }

    public DietModel(string? dietType, double? emissionsPerWeek)
    {
        DietType = dietType;
        EmissionsPerWeek = emissionsPerWeek;
    }

    [JsonProperty("dietType")] public string? DietType { get; set; }
    [JsonProperty("emissionsPerWeek")] public double? EmissionsPerWeek { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}