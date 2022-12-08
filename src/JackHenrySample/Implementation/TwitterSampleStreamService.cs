using JackHenrySample.Configuration;
using JackHenrySample.Data.Twitter;
using JackHenrySample.Interfaces;
using Tweetinvi;
using Tweetinvi.Streaming.V2;

namespace JackHenrySample.Implementation
{
    public class TwitterSampleStreamService : ITwitterStreamService
    {
        private TwitterClient? appClient = null;
        private ISampleStreamV2? sampleStream = null;

        public Task StartStream(Func<TweetData, Task> receivedEvent)
        {
            appClient = new TwitterClient(
                Global.Settings.TwitterConsumerKey,
                Global.Settings.TwitterConsumerSecret,
                Global.Settings.TwitterBearerToken);
            sampleStream = appClient.StreamsV2.CreateSampleStream();
            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                receivedEvent.Invoke(new TweetData
                {
                    Id = eventArgs.Tweet.Id,
                    Text = eventArgs.Tweet.Text,
                    HashTags = eventArgs.Tweet.Entities.Hashtags?.Select(t => t.Tag)?.ToArray()
                });
            };
            return sampleStream.StartAsync()
                .ContinueWith(t => { ContinueStream(receivedEvent); });
        }

        private void ContinueStream(Func<TweetData, Task> receivedEvent)
        {
            StartStream(receivedEvent);
        }

        public void StopStream()
        {
            sampleStream?.StopStream();
        }
    }
}
