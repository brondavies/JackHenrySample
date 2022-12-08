using JackHenrySample.Data.Twitter;
using JackHenrySample.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace JackHenrySample.Hubs
{
    public class TweetStreamingHub : Hub<ITweetStreamingHubClient>
    {
        public static void MapHub(IEndpointRouteBuilder routes) => routes.MapHub<TweetStreamingHub>("/hub/tweets");

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Watchers");
            await base.OnConnectedAsync();
        }

        public async Task SendTweet(TweetData tweet) => await Clients.Group("Watchers").Tweet(tweet);
    }
}
