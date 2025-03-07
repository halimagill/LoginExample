using LoginExample.Data.Models;

namespace LoginExample.BSN.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<string?> Login(string email, string password);
        void LogOut();
    }
}