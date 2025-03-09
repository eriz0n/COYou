using Newtonsoft.Json;

namespace coyou;

public class HistoryModel
{
    public HistoryModel()
    {
    }

    public HistoryModel(DateTime? date, double? co2Emissions)
    {
        Date = date;
        Co2Emissions = co2Emissions;
    }

    [JsonProperty("date")] public DateTime? Date { get; set; }

    [JsonProperty("co2Emissions")] public double? Co2Emissions { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}