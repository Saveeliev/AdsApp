using DTO;
using DTO.AdRequest;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.AdService
{
    public interface IAdService
    {
        Task AddAdvertisement(AddAdvertisementRequest ad, Guid userId);
        Task UpdateAdvertisement(Guid adId, string adText, string adTitle, Guid userId);
        Task Like(Guid adId, Guid userId);
        Task DisLike(Guid adId, Guid userId);
        Task Delete(Guid adId, Guid userId);
        AdDto GetAd(Guid adId);
        List<AdDto> GetAds();
    }
}