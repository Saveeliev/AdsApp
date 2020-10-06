using AdsApp.Models;
using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public LoginController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IActionResult Index()
        {
            return View("~/Views/LoginView.cshtml");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");

            return Redirect("/Home");
        }

        [HttpPost]
        public IActionResult Authenticate(string login, string password)
        {
            var user = new UserDto { Login = login, Password = password };

            var token = Token(user);

            HttpContext.Response.Cookies.Append("access_token", token.Token);

            return Redirect("/Home");
        }

        public TokenDto Token(UserDto user)
        {
            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var token = new TokenDto { Token = encodedJwt, Identity = identity };

            return token;
        }

        private ClaimsIdentity GetIdentity(UserDto user)
        {
            var currentUser = _dataProvider.Get<UserDb>(i => i.Login == user.Login && i.Password == user.Password).SingleOrDefault();

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}