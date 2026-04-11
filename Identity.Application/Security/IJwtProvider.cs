namespace Identity.Application.Security
{
    public interface IJwtProvider
    {
        string GenerateToken(string userId, string username, string role, string email);
    }
}
