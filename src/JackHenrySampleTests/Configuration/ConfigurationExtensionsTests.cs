using Microsoft.Extensions.Configuration;

namespace JackHenrySample.Configuration.Tests
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void AsDictionaryTest()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "Settings:One", "Two" },
                    { "Settings:Test", "Value" },
                    { "Logging:Two", "Three" }
                })
                .Build();

            var result = config.AsDictionary();

            Assert.True(result.ContainsKey("One"));
            Assert.True(result.ContainsKey("Test"));
            Assert.False(result.ContainsKey("Two"));
            Assert.Equal("Two", result["One"]);
            Assert.Equal("Value", result["Test"]);
        }
    }
}