namespace MXC.Models.Login
{
    public sealed record AuthResponse(
        string Token,
        string Email);
}
