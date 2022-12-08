using JackHenrySample.Data.Twitter;

namespace JackHenrySample.Hubs.Clients
{
    public interface ITweetStreamingHubClient
    {
        Task Tweet(TweetData data);
    }
}
