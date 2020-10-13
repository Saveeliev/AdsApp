using DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.AdService
{
    public interface IAdService
    {
        Task AddAdvertisement(AdDto ad, Guid userId);
        Task<IActionResult> UpdateAdvertisement(Guid adId, string adText, string adTitle, Guid userId);
        Task<IActionResult> Like(Guid adId, Guid userId);
        Task<IActionResult> DisLike(Guid adId, Guid userId);
        Task<IActionResult> Delete(Guid adId, Guid userId);
        AdDto GetAd(Guid adId);
        List<AdDto> GetAds();
    }
}