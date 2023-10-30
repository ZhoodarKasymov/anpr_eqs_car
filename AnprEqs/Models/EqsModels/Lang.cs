using Newtonsoft.Json;

namespace AnprEqs.Models.EqsModels;

public class Lang
{
    [JsonProperty("id")]
    public int? Id { get; set; }

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

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("lang")]
    public string Language { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("buttonText")]
    public string ButtonText { get; set; }
}