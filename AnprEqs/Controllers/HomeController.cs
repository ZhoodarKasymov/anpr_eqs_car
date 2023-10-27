using AnprEqs.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AnprEqs.Controllers;

public class HomeController : Controller
{
    private readonly IHubContext<SignalRHub> _hubContext;

    public HomeController(IHubContext<SignalRHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TestWebhook(CustomModel model)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "New data received: " + model.Text);
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }

    public class CustomModel
    {
        public string Text { get; set; }
    }
}