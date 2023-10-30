using Newtonsoft.Json;

namespace AnprEqs.Models.EqsModels;

public class Result
{
    [JsonProperty("customer")]
    public Customer Customer { get; set; }
}