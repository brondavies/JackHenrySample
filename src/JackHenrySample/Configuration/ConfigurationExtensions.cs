namespace JackHenrySample.Configuration
{
    public static class ConfigurationExtensions
    {
        public static Dictionary<string, string> AsDictionary(this IConfiguration config)
        {
            var dict = new Dictionary<string, string>();
            foreach (var value in config.AsEnumerable().Where(v => v.Key.StartsWith("Settings")))
            {
                if (value.Value != null)
                    dict[value.Key.Split(':').Last()] = value.Value;
            }
            return dict;
        }
    }
}
