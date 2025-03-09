using Newtonsoft.Json;

namespace coyou;

public class TypeModel
{
    public TypeModel()
    {
    }

    public TypeModel(string? name, double? gramsPerKilometer)
    {
        Name = name;
        GramsPerKilometer = gramsPerKilometer;
    }

    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("gramsPerKilometer")] public double? GramsPerKilometer { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}