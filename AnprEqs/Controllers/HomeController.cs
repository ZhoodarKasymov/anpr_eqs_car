using System.Text;
using System.Xml.Linq;
using AnprEqs.Hub;
using AnprEqs.Models;
using AnprEqs.Models.EqsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace AnprEqs.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<SignalRHub> _hubContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    private static EventNotificationAlert? _notificationAlert;

    public HomeController
    (
        IHubContext<SignalRHub> hubContext,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<HomeController> logger)
    {
        _hubContext = hubContext;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [Route("/anpr/event")]
    public IActionResult HandleMultipartFormData()
    {
        _logger.LogInformation("Запрос от камеры пришел!");

        if (!MultipartRequestIsValid(out string? xmlData))
        {
            return BadRequest("Не правильный запрос от камеры!");
        }

        try
        {
            var xdoc = XDocument.Parse(xmlData);

            var eventType = (string)xdoc.Root.Element("{http://www.hikvision.com/ver20/XMLSchema}eventType");
            var dateTime = (string)xdoc.Root.Element("{http://www.hikvision.com/ver20/XMLSchema}dateTime");
            var licensePlate = (string)xdoc.Root.Element("{http://www.hikvision.com/ver20/XMLSchema}ANPR")
                ?.Element("{http://www.hikvision.com/ver20/XMLSchema}licensePlate");

            _logger.LogInformation($"Данные из xml: {eventType}, {dateTime}, {licensePlate}");

            if (eventType is "ANPR")
            {
                _notificationAlert = licensePlate is "unknown"
                    ? null
                    : new EventNotificationAlert
                    {
                        LicensePlate = licensePlate,
                        DateTime = dateTime,
                        EventType = eventType
                    };
            }
            else
            {
                _notificationAlert = null;
            }

            return Ok("XML parsed ok");
        }
        catch (Exception ex)
        {
            return BadRequest("Error parsing XML: " + ex.Message);
        }
    }

    [HttpPost]
    [Route("/inline")]
    public async Task<IActionResult> ToGetInline()
    {
        if (_notificationAlert is null) return BadRequest("Номера нету, пустой!");

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

            _notificationAlert = null;
        }

        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }

    #region Private Methods

    private bool MultipartRequestIsValid(out string? xmlData)
    {
        if (Request.HasFormContentType && Request.Form.Files.Count > 0)
        {
            var formFile = Request.Form.Files[0];

            using var reader = new StreamReader(formFile.OpenReadStream());
            xmlData = reader.ReadToEnd();
            return true;
        }

        xmlData = null;
        return false;
    }

    #endregion
}