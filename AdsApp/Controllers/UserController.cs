using DTO.Request;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View("~/Views/User/Register.cshtml");

            var result = await _userService.Register(request);

            if (result == null)
            {
                ViewData["ErrorMessage"] = "User is already exist";
                return View();
            }

            return Redirect("/User/Login");
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var loginResult = _userService.Login(request);

            if (loginResult == null)
            {
                ViewData["ErrorMessage"] = "Invalid data";
                return View();
            }

            HttpContext.Response.Cookies.Append("access_token", loginResult.Token);

            return Redirect("/Ads");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");

            return Redirect("/User/Login");
        }
    }
}