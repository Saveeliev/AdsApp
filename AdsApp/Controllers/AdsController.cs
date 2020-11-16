using AdsApp.Infrastructure.Extensions;
using AdsApp.Models;
using DTO;
using DTO.AdRequest;
using Infrastructure.Options;
using Infrastructure.Services.AdService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    [Authorize]
    public class AdsController : Controller
    {
        private readonly IAdService _adService;
        private readonly UserOptions _userOptions;

        public AdsController(IAdService adService, IOptions<UserOptions> userOptions)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
            _userOptions = userOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SearchRequest request, int currentPageNumber = 1)
        {
            var searchResult = await _adService.Search(request, currentPageNumber);

            var indexViewModel = new IndexViewModel
            {
                Ads = searchResult.Items,

                PageViewModel = new PageViewModel(searchResult.TotalCount, currentPageNumber, _userOptions.PageItemsCountLimit),

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

            return View(ad);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(AddAdvertisementRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!ModelState.IsValid)
                return View();

            var userId = User.Claims.GetUserId();

            try
            {
                await _adService.AddAdvertisement(request, userId);
            }
            catch (Exception ex)
            {
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
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var ad = _adService.GetAd(request.Id);

            if (!ModelState.IsValid)
            {
                ad = new AdDto()
                {
                    Id = request.Id, 
                    Title = request.Title, 
                    Text = request.Text, 
                    CreatedDate = ad.CreatedDate, 
                    ImagePath = ad.ImagePath, 
                    Number = ad.Number, 
                    Ratings = ad.Ratings, 
                    UserId = ad.UserId, 
                    UserName = ad.UserName
                };

                return View(ad);
            }

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
        public IActionResult AdSinglePage(Guid adId)
        {
            var ad = _adService.GetAd(adId);

            return View(ad);
        }
    }
}