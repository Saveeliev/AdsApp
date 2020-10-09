using AdsApp.DTO;
using AdsApp.Models;
using AdsApp.Models.DTO;
using AdsApp.Models.DTO.ActionResult;
using AdsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp
{
    public class UserService : IUserService
    {
        private readonly IDataProvider _dataProvider;

        public UserService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (IsUserExist(request))
                return null;

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var userDb = new UserDb
            {
                Login = request.Login.ToLower(),
                Name = request.Name,
                Password = hash
            };

            await _dataProvider.Insert(userDb);

            return new EmptyResult();
        }

        public LoginResult Login(LoginRequest request)
        {
            request.Login = request.Login.ToLower();

            var user = _dataProvider.Get<UserDb>(i => i.Login == request.Login).SingleOrDefault();

            if (user == null)
                return null;

            var varify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!varify)
                return null;

            request.UserId = user.Id;

            var token = Token(request);

            return new LoginResult { Token = token };
        }

        public bool IsUserExist(RegisterRequest request)
        {
            var result = _dataProvider.Get<UserDb>(i => i.Login == request.Login).SingleOrDefault();

            if (result != null)
                return true;

            return false;
        }

        public string Token(LoginRequest request)
        {
            var identity = GetIdentity(request);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(LoginRequest request)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, request.UserId.ToString())
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}