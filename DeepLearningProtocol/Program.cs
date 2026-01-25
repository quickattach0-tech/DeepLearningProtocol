using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DeepLearningProtocol.SignalR;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Main entry point for the Deep Learning Protocol application.
    /// Hosts a SignalR endpoint and continues to provide the interactive menu.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Enable SignalR
            builder.Services.AddSignalR();

            // Configure web host URLs
            builder.WebHost.UseUrls("http://0.0.0.0:80");

            var app = builder.Build();

            // Health endpoint
            app.MapGet("/health", () => Results.Ok(new { version = "3.2" }));

            // SignalR Hub
            app.MapHub<NotificationHub>("/hub/notifications");

            // Start web host without blocking
            var webHostTask = app.RunAsync();

            // Run existing interactive menu on the main thread
            MenuSystem.DisplayMainMenu();

            // Wait for the web host to finish (it won't unless stopped)
            await webHostTask;
        }
    }
}


