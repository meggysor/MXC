namespace MXC.Extension
{
    public interface IConfigurationGetter
    {
        string JWTKey { get; }
        string JWTIssuer { get; }
        string JWTAudience { get; }

        int JwtExpirationInMinutes { get; }
    }
}
