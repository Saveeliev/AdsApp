using AdsApp.Extensions;
using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
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
            var ads = _adService.GetAds();
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

            if (ad.UserId != User.Claims.GetUserId())
                return RedirectToAction("Index");

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

            var userId = User.Claims.GetUserId();

            await _adService.AddAdvertisement(ad, userId);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAdvertisement(string adText, Guid adId)
        { 
            if (!ModelState.IsValid)
                return View();

            var result = await _adService.UpdateAdvertisement(adId, adText, User.Claims.GetUserId());

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(Guid adId)
        {
            var result = await _adService.Like(adId, User.Claims.GetUserId());

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DisLike(Guid adId)
        {
            var result = await _adService.DisLike(adId, User.Claims.GetUserId());

            return RedirectToAction("Index");
        }
    }
}