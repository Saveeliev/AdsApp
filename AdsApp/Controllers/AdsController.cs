using AdsApp.Models.DTO.ActionResult;
using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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
            var ads = _adService.GetAllAds();
            return View(ads);
        }

        [Authorize]
        public IActionResult AddAdvertisement()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateAdvertisement(Guid adId)
        {
            var ad = _adService.GetAd(adId);

            ViewData["AdId"] = adId.ToString();
            ViewData["AdText"] = ad.Text;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(AdDto ad)
        {
            if (!ModelState.IsValid)
                return View();

            var userId = User.Claims.Single(i => i.Type == ClaimsIdentity.DefaultNameClaimType).Value;

            ad.UserId = Guid.Parse(userId);

            await _adService.AddAdvertisement(ad);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAdvertisement(string adText, string adId)
        {
            var ad = new AdDto { Id = Guid.Parse(adId), Text = adText };

            if (!ModelState.IsValid)
                return View();

            await _adService.UpdateAdvertisement(ad);

            return RedirectToAction("Index");
        }
    }
}