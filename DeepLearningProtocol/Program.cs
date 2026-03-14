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
            var protocol = new DeepLearningProtocol();
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub")
                .Build();

            connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
                // Agent thinking: process the message through the protocol
                var result = protocol.ExecuteProtocol(message, "Analyze message", 1);
                Console.WriteLine($"{agentName} analysis: {result}");
                // Could send response back to chat
                await connection.InvokeAsync("SendMessage", agentName, $"Analysis: {result.Substring(0, Math.Min(100, result.Length))}");
            });

            connection.On<string, string>("ReceiveVote", (agent, vote) =>
            {
                Console.WriteLine($"{agent} voted: {vote}");
            });

            connection.On("ConferenceStarted", async () =>
            {
                Console.WriteLine("Conference started! Agent is thinking...");
                // Perform reasoning to decide vote
                var reasoning = protocol.ExecuteProtocol("Should I vote yes on this proposal?", "Make voting decision", 2);
                string vote = reasoning.Contains("yes") || reasoning.Contains("approve") ? "Yes" : "No";
                await connection.InvokeAsync("SendVote", agentName, vote);
            });

            await connection.StartAsync();
            Console.WriteLine($"{agentName} connected. Press any key to exit.");
            Console.ReadKey();
            await connection.StopAsync();
        }
    }
}


