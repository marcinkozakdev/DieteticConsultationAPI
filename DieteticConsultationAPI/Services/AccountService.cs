﻿using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DieteticConsultationAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly DieteticConsultationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAccountRepository _accountRepository;

        public AccountService(DieteticConsultationDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IAccountRepository accountRepository)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _accountRepository = accountRepository;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context
                .Users
                .Include(u=>u.Role)
                .FirstOrDefault(x => x.Email == dto.Email);

            if (user is null)
            {
                throw new BadReguestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadReguestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHendler = new JwtSecurityTokenHandler();
            return tokenHendler.WriteToken(token);
        }
    }
}