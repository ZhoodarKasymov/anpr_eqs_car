using Newtonsoft.Json;

namespace AnprEqs.Models.EqsModels;

public class Customer
{
    [JsonProperty("id")]
    public long? Id { get; set; }

    [JsonProperty("number")]
    public int? Number { get; set; }

    [JsonProperty("stateIn")]
    public int? StateIn { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("priority")]
    public int? Priority { get; set; }

    [JsonProperty("to_service")]
    public ToService ToService { get; set; }

    [JsonProperty("prefix")]
    public string Prefix { get; set; }

    [JsonProperty("stand_time")]
    public string StandTime { get; set; }

    [JsonProperty("inputData")]
    public string InputData { get; set; }

    [JsonProperty("need_back")]
    public bool? NeedBack { get; set; }

    [JsonProperty("temp_comments")]
    public string TempComments { get; set; }

    [JsonProperty("post_status")]
    public string PostStatus { get; set; }

    [JsonProperty("postpone_period")]
    public int? PostponePeriod { get; set; }

    [JsonProperty("recall_cnt")]
    public int? RecallCnt { get; set; }

    [JsonProperty("start_postpone_period")]
    public int? StartPostponePeriod { get; set; }

    [JsonProperty("finish_postpone_period")]
    public int? FinishPostponePeriod { get; set; }

    [JsonProperty("complex_id")]
    public List<object> ComplexId { get; set; }
}