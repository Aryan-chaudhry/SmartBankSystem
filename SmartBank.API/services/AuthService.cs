using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class AuthService: IAuthService
    {
        private readonly SmartBankContext _context;
        private readonly IConfiguration _config;
        public AuthService(SmartBankContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

    

        public async Task<RegisteruserDTO> RegisterAsync(RegisteruserDTO request)
        {
            if(await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }
            
            var user = new User();

            var hashPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.CreatedAt = DateTime.UtcNow;
            user.PasswordHash = hashPassword;
            user.NationalId = request.NationalId;
            

            var role = await _context.Roles.FindAsync(request.RoleId);

            user.Role = role;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return request;

        }

        public async Task<string> LoginAsync(LoginuserDTO request)
        {
            // user not founf
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if(user is null)
            {
                 return null;
            }

            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            // claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Customer")
            };

            Console.WriteLine(claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")!));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var tokendescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(tokendescriptor);
        }
    }
}