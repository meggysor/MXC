using System.Collections.Concurrent;

namespace MXC.Extension
{
    public class ConfigurationGetter : IConfigurationGetter
    {
        private static readonly ConcurrentDictionary<string, string> _stringConfigDictionary = new();
        private static readonly ConcurrentDictionary<string, int> _intConfigDictionary = new();

        public string JWTKey => GetStringConfiguration("Jwt:Key");
        public string JWTIssuer => GetStringConfiguration("Jwt:Issuer");
        public string JWTAudience => GetStringConfiguration("Jwt:Audience");
        public int JwtExpirationInMinutes => GetIntConfiguration("Jwt:ExpiryMinutes");


        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables();

            return builder.Build();
        }

        public static string GetStringConfigurationValue(string key)
        {
            var envirVar = System.Environment.GetEnvironmentVariable($"APPSETTING_{key}");
            if (string.IsNullOrWhiteSpace(envirVar))
            {
                var configuration = BuildConfiguration();
                envirVar = configuration[key];
            }

            return envirVar;
        }

        private string GetStringConfiguration(string key)
        {
            if (_stringConfigDictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            var configValue = GetStringConfigurationValue(key) ?? string.Empty;
            _stringConfigDictionary.TryAdd(key, configValue);

            return configValue;
        }

        private int GetIntConfiguration(string key)
        {
            if (_intConfigDictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            var featureFlag = (GetStringConfigurationValue(key) ?? string.Empty).Trim();
            var configValue =  int.TryParse(featureFlag, out var result) ? result : 0;
            _intConfigDictionary.TryAdd(key, configValue);

            return configValue;
        }
    }
}
