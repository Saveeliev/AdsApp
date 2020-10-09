using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class AdsController : Controller
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddAdvertisement()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(AdDto ad)
        {
            var userId = User.Claims.Single(i => i.Type == ClaimsIdentity.DefaultNameClaimType).Value;

            ad.UserId = Guid.Parse(userId);

            await _adService.AddAdvertisement(ad);

            return RedirectToAction("Index");
        }
    }
}