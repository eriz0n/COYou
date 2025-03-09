using Newtonsoft.Json;

namespace coyou;

public class HistoryRequestModel
{
    public HistoryRequestModel()
    {
    }

    public HistoryRequestModel(DateOnly? startDate, DateOnly? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    [JsonProperty("startDate")] public DateOnly? StartDate { get; set; }
    [JsonProperty("endDate")] public DateOnly? EndDate { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}