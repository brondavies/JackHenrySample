namespace JackHenrySample.Configuration
{
    public class StandardSettings
    {
        private Dictionary<string, string>? dictionary;

        public StandardSettings() { }
        public StandardSettings(Dictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public int TopHashtagCount => Convert.ToInt32(GetString("TopHashtagCount", "10"));

        public string? TwitterBearerToken => GetString("TwitterBearerToken");

        public string? TwitterConsumerKey => GetString("TwitterConsumerKey");

        public string? TwitterConsumerSecret => GetString("TwitterConsumerSecret");

        public string? GetString(string key, string? defaultValue = null)
        {
            if (dictionary?.TryGetValue(key, out string? value) == true)
            {
                return value;
            }
            return defaultValue;
        }
    }
}
