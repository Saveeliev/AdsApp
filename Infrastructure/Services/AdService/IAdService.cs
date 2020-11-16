using DTO;
using DTO.ActionResult;
using DTO.AdRequest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.AdService
{
    public interface IAdService
    {
        Task AddAdvertisement(AddAdvertisementRequest ad, Guid userId);
        Task UpdateAdvertisement(AdvertisementRequest request, Guid userId);
        Task Like(Guid adId, Guid userId);
        Task DisLike(Guid adId, Guid userId);
        Task Delete(Guid adId, Guid userId);
        AdDto GetAd(Guid adId);
        Task<SearchResult<AdDto>> Search(SearchRequest request, int currentPageNumber);
    }
}