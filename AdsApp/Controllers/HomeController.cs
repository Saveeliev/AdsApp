using AdsApp.DTO;
using AdsApp.Models;
using AdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdService _adService;

        public HomeController(ILogger<HomeController> logger, IAdService adService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }

        [Authorize]
        public IActionResult Index()
        {
            var ads = _adService.GetAds();
            return View("~/Views/UserInterfaceView.cshtml", ads);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}