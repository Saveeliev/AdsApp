using AdsApp.Extensions;
using DTO;
using DTO.AdRequest;
using Infrastructure.Services.AdService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    [Authorize]
    public class AdsController : Controller
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }

        public IActionResult Index()
        {
            var ads = _adService.GetAds();
            return View(ads);
        }

        public IActionResult AddAdvertisement()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAdvertisement(Guid adId)
        {
            var ad = _adService.GetAd(adId);

            if (ad.UserId != User.Claims.GetUserId())
            {
                return RedirectToAction("Index");
            }

            ViewData["AdId"] = adId.ToString();
            ViewData["AdText"] = ad.Text;
            ViewData["AdTitle"] = ad.Title;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(AddAdvertisementRequest ad)
        {
            if (!ModelState.IsValid)
                return View();

            var userId = User.Claims.GetUserId();

            try
            {
                await _adService.AddAdvertisement(ad, userId);
            }
            catch(Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View();
            }

            ViewData["Message"] = "Your ad added.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdvertisement(string adTitle, string adText, Guid adId)
        { 
            if (!ModelState.IsValid)
                return View();

            try
            {
                await _adService.UpdateAdvertisement(adId, adText, adTitle, User.Claims.GetUserId());
            }
            catch(UnauthorizedAccessException)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Like(Guid adId)
        {
            await _adService.Like(adId, User.Claims.GetUserId());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DisLike(Guid adId)
        {
            await _adService.DisLike(adId, User.Claims.GetUserId());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid adId)
        {
            var userId = User.Claims.GetUserId();

            try
            {
                await _adService.Delete(adId, userId);
            }
            catch(UnauthorizedAccessException)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SinglePage(Guid adId)
        {
            var ad = _adService.GetAd(adId);

            return View(ad);
        }
    }
}