using AdsApp.Extensions;
using AdsApp.Models;
using DTO;
using DTO.AdRequest;
using Infrastructure.Options;
using Infrastructure.Services.AdService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> Index(SearchRequest request, int currentPageNumber = 1)
        {
            var ads = await _adService.Search(request);

            var indexViewModel = new IndexViewModel
            {
                Ads = ads.Skip((currentPageNumber - 1) * UserOptions.PageAdsCount)
                .Take(UserOptions.PageAdsCount)
                .ToArray(),

                PageViewModel = new PageViewModel(ads.Length, currentPageNumber, UserOptions.PageAdsCount),

                SearchRequest = request
            };

            return View(indexViewModel);
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

            var request = new AdvertisementRequest { Id = ad.Id, Text = ad.Text, Title = ad.Title };

            return View(ad);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(AddAdvertisementRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var userId = User.Claims.GetUserId();

            try
            {
                await _adService.AddAdvertisement(request, userId);
            }
            catch (Exception ex)
            {
                var AdAdvertisimentModal = new PartialModel
                {
                    PartialViewName = "AddAdvertisimentModalView",
                    Model = ex.Message
                };

                ViewBag.Message = ex.Message;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult OpenImage(string adImagePath)
        {
            var openImageModal = new PartialModel
            {
                PartialViewName = "OpenImageModalView",
                Model = adImagePath
            };

            return PartialView("ModalView", openImageModal);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdvertisement(AdvertisementRequest request)
        {
            var ad = _adService.GetAd(request.Id);

            if (!ModelState.IsValid)
                return View(ad);

            try
            {
                await _adService.UpdateAdvertisement(request, User.Claims.GetUserId());
            }
            catch (UnauthorizedAccessException)
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
        public IActionResult OpenWindowToDelete(Guid adId)
        {
            var deleteAdModel = new PartialModel
            {
                PartialViewName = "DeleteAdModalView",
                Model = adId
            };

            return PartialView("ModalView", deleteAdModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid adId)
        {
            var userId = User.Claims.GetUserId();

            try
            {
                await _adService.Delete(adId, userId);
            }
            catch (UnauthorizedAccessException)
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