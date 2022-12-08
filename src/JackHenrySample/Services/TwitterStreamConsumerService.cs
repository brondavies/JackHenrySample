using JackHenrySample.Configuration;
using JackHenrySample.Data.Twitter;
using JackHenrySample.Hubs;
using JackHenrySample.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Extensions;

namespace JackHenrySample.Services
{
    public class TwitterStreamConsumerService : IHostedService
    {
        readonly IHubContext<TweetStreamingHub> HubContext;

        readonly ILogger<TwitterStreamConsumerService> Log;

        readonly ITwitterStreamService TwitterStreamService;
        readonly Regex emojiRegex = EmojiData.Emoji.EmojiRegex;

        long LastStatisticsUpdate = 0;
        int HashtagCount = Global.Settings.TopHashtagCount;

        public ConcurrentDictionary<string, long> stats = new();
        public ConcurrentDictionary<string, long> emoji = new();
        public TwitterStreamConsumerService(
            ITwitterStreamService twitterStreamService,
            IHubContext<TweetStreamingHub> hubContext, 
            ILogger<TwitterStreamConsumerService> logger
            )
        {
            TwitterStreamService = twitterStreamService;
            HubContext = hubContext;
            Log = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                TwitterStreamService.StartStream(async tweet =>
                {
                    try
                    {
                        await ProcessTweet(tweet);
                    }
                    catch (Exception e)
                    {
                        Log.LogError(e, "Tweet processing error");
                    }
                });
            }, cancellationToken);
        }

        private async Task ProcessTweet(TweetData tweet)
        {
            await HubContext.Clients.All.SendAsync("tweet", tweet);
            LastStatisticsUpdate++;
            tweet.HashTags?.ForEach(hashtag =>
            {
                Increment(stats, hashtag);
            });
            foreach (var match in emojiRegex.Matches(tweet.Text ?? ""))
            {
                Increment(emoji, $"{match}");
            }
            if (LastStatisticsUpdate >= 100)
            {
                LastStatisticsUpdate = 0;
                var hashtags = stats
                    .OrderByDescending(s => s.Value)
                    .ThenBy(s => s.Key)
                    .Take(HashtagCount)
                    .ToArray();
                var emojis = emoji
                    .OrderByDescending(s => s.Value)
                    .ThenBy(s => s.Key)
                    .Take(HashtagCount)
                    .ToArray();
                await HubContext.Clients.All.SendAsync("stats", new { hashtags, emojis });
            }
        }

        private void Increment(ConcurrentDictionary<string, long> dict, string m)
        {
            if (dict.TryGetValue(m, out var value))
                dict[m] = value + 1;
            else
                dict[m] = 1;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TwitterStreamService.StopStream();
            return Task.CompletedTask;
        }
    }
}
