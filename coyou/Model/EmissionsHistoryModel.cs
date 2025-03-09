using Newtonsoft.Json;

namespace coyou;

public class EmissionsHistoryModel
{
    public EmissionsHistoryModel()
    {
    }

    public EmissionsHistoryModel(string? calendarWeek, double? emissions)
    {
        CalendarWeek = calendarWeek;
        Emissions = emissions;
    }

    [JsonProperty("calendarWeek")] public string? CalendarWeek { get; set; } 
    [JsonProperty("emissions")] public double? Emissions { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}