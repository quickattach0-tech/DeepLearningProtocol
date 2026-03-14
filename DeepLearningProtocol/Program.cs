using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Main entry point for the Deep Learning Protocol application.
    /// Delegates to MenuSystem for interactive menu display and protocol execution.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "agent")
            {
                await RunAsAgent(args);
            }
            else
            {
                MenuSystem.DisplayMainMenu();
            }
        }

        static async Task RunAsAgent(string[] args)
        {
            string agentName = args.Length > 1 ? args[1] : "Agent1";
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub")
                .Build();

            connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
                // Agent thinking logic here
                // Use the DeepLearningProtocol reasoning
            });

            connection.On<string, string>("ReceiveVote", (agent, vote) =>
            {
                Console.WriteLine($"{agent} voted: {vote}");
            });

            connection.On("ConferenceStarted", async () =>
            {
                Console.WriteLine("Conference started! Agent is thinking...");
                // Perform reasoning and vote
                // For demo, send a vote
                await connection.InvokeAsync("SendVote", agentName, "Yes");
            });

            await connection.StartAsync();
            Console.WriteLine($"{agentName} connected. Press any key to exit.");
            Console.ReadKey();
            await connection.StopAsync();
        }
    }
}


