using System.Text;
using System.Xml.Serialization;
using AnprEqs.Hub;
using AnprEqs.Models;
using AnprEqs.Models.EqsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace AnprEqs.Controllers;

public class HomeController : Controller
{
    private readonly IHubContext<SignalRHub> _hubContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    private static EventNotificationAlert? _notificationAlert;

    public HomeController(IHubContext<SignalRHub> hubContext, IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _hubContext = hubContext;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [Route("/anpr/event")]
    public async Task ParseWebhook()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var requestBody = await reader.ReadToEndAsync();

        var serializer = new XmlSerializer(typeof(EventNotificationAlert));
        using var textReader = new StringReader(requestBody);

        var alert = (EventNotificationAlert)serializer.Deserialize(textReader)!;

        if (alert.EventType is "ANPR")
        {
            _notificationAlert = alert.LicensePlate is "unknown" ? null : alert;
        }
        else
        {
            _notificationAlert = null;
        }
    }

    [HttpPost]
    [Route("/inline")]
    public async Task<IActionResult> ToGetInline()
    {
        if (_notificationAlert is null) return BadRequest();

        var serviceId = _configuration["ServiceId"];
        var serverUrl = _configuration["ServerUrl"];

        var httpClient = _httpClientFactory.CreateClient();

        var requestUri = new Uri(serverUrl!);

        // Define the request content as JSON
        var content = new StringContent(
            "{\"jsonrpc\": \"2.0\", \"method\": \"Поставить в очередь\", \"params\": {\"service_id\": \"" + serviceId +
            "\", \"text_data\": \"" + _notificationAlert.LicensePlate + "\"}}",
            Encoding.UTF8,
            "application/json"
        );

        // Send the HTTP POST request
        var response = await httpClient.PostAsync(requestUri, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var desResponse = JsonConvert.DeserializeObject<ResponseEqs>(responseContent);
            var customer = desResponse.Result.Customer;
            
            await _hubContext.Clients.All.SendAsync("AnprUpdates", new CarInlineViewModel
            {
                ServiceName = customer.ToService.Name,
                LicensePlate = customer.InputData,
                Date = customer.StandTime,
                Talon = $"{customer.Prefix}{customer.Number}"
            });
        }
        
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}