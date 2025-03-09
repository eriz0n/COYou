using Newtonsoft.Json;

namespace coyou;

public class RouteModel
{
    public RouteModel()
    {
    }

    public RouteModel(string? start, string? end, string? movementType, double? lengthKm)
    {
        Start = start;
        End = end;
        MovementType = movementType;
        LengthKm = lengthKm;
    }

    [JsonProperty("start")] public string? Start { get; set; } 
    [JsonProperty("end")] public string? End { get; set; }
    [JsonProperty("movementType")] public string? MovementType { get; set; }
    [JsonProperty("lengthKm")] public double? LengthKm { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}