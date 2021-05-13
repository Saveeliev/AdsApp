using System.Threading.Tasks;
using DTO.AdRequest;
using Infrastructure.Services.AdService;
using Microsoft.AspNetCore.Mvc;

namespace AdsApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAds(SearchRequest request)
        {
            var searchResult = await _adService.Search(request, 1);
            return new ObjectResult(searchResult);
        }
    }
}