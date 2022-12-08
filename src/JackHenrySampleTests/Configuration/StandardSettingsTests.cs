namespace JackHenrySample.Configuration.Tests
{
    public class StandardSettingsTests
    {
        [Fact]
        public void GetStringTest()
        {
            var settings = new StandardSettings(new Dictionary<string, string> {
                { "Test1", "Jack" },
                { "Test2", "Henry" }
            });
            Assert.Equal("Jack", settings.GetString("Test1"));
            Assert.Equal("Henry", settings.GetString("Test2"));
            Assert.Equal("", settings.GetString("Test3", ""));
            Assert.Equal("Associates", settings.GetString("Test4", "Associates"));
            Assert.Null(settings.GetString("Missing"));
        }
    }
}