using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginExample.BSN.Interfaces;
using LoginExample.Data;
using LoginExample.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LoginExample.BSN.Services
{
    public class AuthService : IAuthService
    {
        private string _privateKey;
        private SignInManager<AppUser> _signInManager;
        private IUserManagerService _userManagerService;
        public AuthService(string privateKey, SignInManager<AppUser> signInMgr, IUserManagerService userManagerService)
        {
            _privateKey = privateKey;
            _signInManager = signInMgr;
            _userManagerService = userManagerService;
        }

        public string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_privateKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            //ADD BACK ONCE YOU SET UP ROLES LOGIC
            //foreach (var role in user.Roles)
            //    claims.AddClaim(new Claim(ClaimTypes.Role, role));
            
            return claims;
        }

        public async Task<string?> Login(string email, string password)
        {
            var findUser = await _userManagerService.FindUserByEmail(email);

            if (findUser is null)
            {
                throw new Exception("The user was not found");
            }

            bool isPasswordVerified = await _userManagerService.VerifyPassword(findUser, password);

            if (!isPasswordVerified)
            {
                throw new Exception("The password is incorrect");
            }

            var token = GenerateToken(findUser.ToDTO());

            return token;
        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
