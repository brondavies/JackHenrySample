using JackHenrySample.Data.Twitter;

namespace JackHenrySample.Interfaces
{
    public interface ITwitterStreamService
    {
        Task StartStream(Func<TweetData, Task> receivedEvent);
        void StopStream();
    }
}
