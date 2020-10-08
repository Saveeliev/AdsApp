using AdsApp.DTO;
using AdsApp.Models;
using AdsApp.Models.DTO;
using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;

        public UserController(IUserService userService, IServiceProvider serviceProvider)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var context = _serviceProvider.GetRequiredService<AdsAppContext>();

            using var transaction = context.Database.BeginTransaction(IsolationLevel.RepeatableRead);

            if (!_userService.Login(request))
            {
                ViewData["ErrorMessage"] = "Invalid data";
                return View();
            }

            var token = Token(request);

            HttpContext.Response.Cookies.Append("access_token", token);

            transaction.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View("~/Views/User/Register.cshtml");

            var context = _serviceProvider.GetRequiredService<AdsAppContext>();

            using var transaction = context.Database.BeginTransaction(IsolationLevel.RepeatableRead);

            if (_userService.IsUserExist(request))
            {
                ViewData["ErrorMessage"] = "User is already exist";
                return View();
            }

            await _userService.Register(request);

            transaction.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");

            return RedirectToAction("Index");
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
                    new Claim(ClaimsIdentity.DefaultNameClaimType, request.Login)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
