using AdsApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IAdService
    {
        Task AddAdvertisement(AdDto ad);
        Task UpdateAdvertisement(AdDto ad);
        AdDto GetAd(Guid adId);
        List<AdDto> GetAllAds();
    }
}