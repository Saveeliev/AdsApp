using AdsApp.Models.ViewModels;
using AdsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddAdvertisement()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> AddAdvertisement(string text)
        {
            var userClaims = User.Claims;
            string userName;

            foreach(var claim in userClaims)
            {
                if(claim.Type == ClaimsIdentity.DefaultNameClaimType)
                {
                    userName = claim.Value;

                    var ad = new AdDto { Text = text, UserName = userName };
                    await _adService.AddAdvertisement(ad);
                }
            }

            return "Success!";
        }
    }
}