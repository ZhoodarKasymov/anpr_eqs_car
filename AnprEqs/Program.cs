using System.Net;
using AnprEqs.Hub;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Configure log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Add global exception handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/html";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Log the exception
        var logger = LogManager.GetLogger(exceptionHandlerPathFeature?.Path);
        logger.Error("Ошибка: ", exception);

        if (exceptionHandlerPathFeature?.Error is NullReferenceException)
        {
            context.Response.Redirect("/");
            return;
        }

        await context.Response.WriteAsync(
            exception?.Message ?? "Произошла ошибка. Пожалуйста, повторите попытку позже.");
    });
});

app.MapHub<SignalRHub>("/signalRHub"); // Map the hub

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();