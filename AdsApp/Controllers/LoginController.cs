using AdsApp.Models;
using AdsApp.Models.DTO;
using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IUserService _userService;

        public LoginController(IDataProvider dataProvider, IUserService userService)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        public IActionResult Authenticate(LoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View("~/Views/LoginView.cshtml");
            }

            if (!_userService.IsUserExist(request))
                return View("~/Views/LoginView.cshtml", "User is not exist");

            var token = Token(request);

            HttpContext.Response.Cookies.Append("access_token", token.Token);

            return Redirect("/Home");
        }

        public TokenDto Token(LoginRequest request)
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

            var token = new TokenDto { Token = encodedJwt, Identity = identity };

            return token;
        }

        private ClaimsIdentity GetIdentity(LoginRequest request)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, request.Login)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}