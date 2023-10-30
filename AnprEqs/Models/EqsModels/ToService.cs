using Newtonsoft.Json;

namespace AnprEqs.Models.EqsModels;

public class ToService
{
    [JsonProperty("id")]
    public long? Id { get; set; }

    [JsonProperty("status")]
    public int? Status { get; set; }

    [JsonProperty("point")]
    public int? Point { get; set; }

    [JsonProperty("duration")]
    public int? Duration { get; set; }

    [JsonProperty("exp")]
    public int? Exp { get; set; }

    [JsonProperty("sound_template")]
    public string SoundTemplate { get; set; }

    [JsonProperty("advance_limit")]
    public int? AdvanceLimit { get; set; }

    [JsonProperty("day_limit")]
    public int? DayLimit { get; set; }

    [JsonProperty("person_day_limit")]
    public int? PersonDayLimit { get; set; }

    [JsonProperty("inputed_as_number")]
    public int? InputedAsNumber { get; set; }

    [JsonProperty("advance_limit_period")]
    public int? AdvanceLimitPeriod { get; set; }

    [JsonProperty("advance_time_period")]
    public int? AdvanceTimePeriod { get; set; }

    [JsonProperty("enable")]
    public int? Enable { get; set; }

    [JsonProperty("result_required")]
    public bool? ResultRequired { get; set; }

    [JsonProperty("input_required")]
    public bool? InputRequired { get; set; }

    [JsonProperty("inputed_as_ext")]
    public bool? InputedAsExt { get; set; }

    [JsonProperty("input_caption")]
    public string InputCaption { get; set; }

    [JsonProperty("pre_info_html")]
    public string PreInfoHtml { get; set; }

    [JsonProperty("pre_info_print_text")]
    public string PreInfoPrintText { get; set; }

    [JsonProperty("ticket_text")]
    public string TicketText { get; set; }

    [JsonProperty("tablo_text")]
    public string TabloText { get; set; }

    [JsonProperty("but_x")]
    public int? ButX { get; set; }

    [JsonProperty("but_y")]
    public int? ButY { get; set; }

    [JsonProperty("but_b")]
    public int? ButB { get; set; }

    [JsonProperty("but_h")]
    public int? ButH { get; set; }

    [JsonProperty("countPerDay")]
    public int? CountPerDay { get; set; }

    [JsonProperty("day")]
    public int? Day { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("service_prefix")]
    public string ServicePrefix { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("buttonText")]
    public string ButtonText { get; set; }

    [JsonProperty("parentId")]
    public int? ParentId { get; set; }

    [JsonProperty("langs")]
    public List<Lang> Langs { get; set; }

    [JsonProperty("inner_services")]
    public List<object> InnerServices { get; set; }
}