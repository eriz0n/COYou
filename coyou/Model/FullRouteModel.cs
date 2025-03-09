using Newtonsoft.Json;

namespace coyou;

public class FullRouteModel
{
    public FullRouteModel()
    {
    }

    public FullRouteModel(long? id, string? start, string? end, string? movementType, double? lengthKm, double? emissions)
    {
        Id = id;
        Start = start;
        End = end;
        MovementType = movementType;
        LengthKm = lengthKm;
        Emissions = emissions;
    }

    [JsonProperty("id")] public long? Id { get; set; }
    [JsonProperty("start")] public string? Start { get; set; }
    [JsonProperty("end")] public string? End { get; set; }
    [JsonProperty("movementType")] public string? MovementType { get; set; }
    [JsonProperty("lengthKm")] public double? LengthKm { get; set; }
    [JsonProperty("emissions")] public double? Emissions { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}