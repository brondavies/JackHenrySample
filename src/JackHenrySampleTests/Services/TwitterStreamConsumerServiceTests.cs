using Moq;
using JackHenrySample.Interfaces;
using JackHenrySample.Data.Twitter;
using Microsoft.AspNetCore.SignalR;
using JackHenrySample.Hubs;
using Microsoft.Extensions.Logging;

namespace JackHenrySample.Services.Tests
{
    public class TwitterStreamConsumerServiceTests
    {
        [Fact]
        public void StartAsyncTest()
        {
            var twitterStreamService = new MockITwitterStreamService();
            var hubContext = MockHubContext();
            var logger = Mock.Of<ILogger<TwitterStreamConsumerService>>(MockBehavior.Loose);
            var service = new TwitterStreamConsumerService(twitterStreamService, hubContext, logger);

            service.StartAsync(new CancellationToken()).Wait();

            Assert.Equal(1, service.stats["one"]);
            Assert.Equal(1, service.stats["two"]);
        }

        private IHubContext<TweetStreamingHub> MockHubContext()
        {
            var mock = new Mock<IHubContext<TweetStreamingHub>>();
            var clients = new Mock<IHubClients>();
            clients.SetupGet(m => m.All).Returns(Mock.Of<IClientProxy>(MockBehavior.Loose));
            mock.SetupGet(m => m.Clients).Returns(clients.Object);

            return mock.Object;
        }
    }

    internal class MockITwitterStreamService : ITwitterStreamService
    {
        public Task StartStream(Func<TweetData, Task> receivedEvent)
        {
            receivedEvent.Invoke(new TweetData
            {
                Id = "1",
                HashTags = new[] { "one", "two" },
                Text = "Thanks 🙂"
            });
            return Task.FromResult(0);
        }

        public void StopStream()
        {
            throw new NotImplementedException();
        }
    }
}