using Microsoft.AspNetCore.SignalR;

namespace DeepLearningProtocol.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendVote(string agent, string vote)
        {
            await Clients.All.SendAsync("ReceiveVote", agent, vote);
        }

        public async Task SendRating(string agent, string rating)
        {
            await Clients.All.SendAsync("ReceiveRating", agent, rating);
        }

        public async Task StartConference()
        {
            await Clients.All.SendAsync("ConferenceStarted");
        }

        public async Task SendImageProcessingLog(string log)
        {
            await Clients.All.SendAsync("ReceiveImageLog", log);
        }
    }
}