
using DataBase.Models;
using DTO;
using Infrastructure.Services.DataProvider;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.AdService
{
    public class AdService : IAdService
    {
        private readonly IDataProvider _dataProvider;

        public AdService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task AddAdvertisement(AdDto ad, Guid userId)
        {
            byte[] image = null;

            if (ad.Image != null)
            {
                using var binaryReader = new BinaryReader(ad.Image.OpenReadStream());
                image = binaryReader.ReadBytes((int)ad.Image.Length);
            }

            var currentAd = new AdDb { CreatedDate = DateTime.Now, Image = image, Title = ad.Title, Text = ad.Text, UserId = userId };

            await _dataProvider.Insert(currentAd);
        }

        public async Task<IActionResult> UpdateAdvertisement(Guid adId, string adText, string adTitle, Guid userId)
        {
            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == adId).SingleOrDefault();

            if (currentAd.UserId != userId)
                return null;

            currentAd.Text = adText;
            currentAd.Title = adTitle;

            await _dataProvider.Update(currentAd);

            return new OkResult();
        }

        public async Task<IActionResult> Like(Guid adId, Guid userId)
        {
            var isLiked = _dataProvider.Get<RatingDb>(i => i.AdId == adId && i.UserId == userId).SingleOrDefault();

            if(isLiked == null)
            {
                var like = new RatingDb { AdId = adId, UserId = userId, IsLiked = true };
                await _dataProvider.Insert(like);
            }
            else
            {
                if (isLiked.IsLiked == true)
                    await _dataProvider.Delete(isLiked);

                else
                {
                    isLiked.IsLiked = true;
                    await _dataProvider.Update(isLiked);
                }

            }

            return new OkResult();
        }

        public async Task<IActionResult> DisLike(Guid adId, Guid userId)
        {
            var isLiked = _dataProvider.Get<RatingDb>(i => i.AdId == adId && i.UserId == userId).SingleOrDefault();

            if (isLiked == null)
            {
                var disLike = new RatingDb { AdId = adId, UserId = userId, IsLiked = false };
                await _dataProvider.Insert(disLike);
            }
            else
            {
                if (isLiked.IsLiked == false)
                    await _dataProvider.Delete(isLiked);

                else
                {
                    isLiked.IsLiked = false;
                    await _dataProvider.Update(isLiked);
                }
            }

            return new OkResult();
        }

        public async Task<IActionResult> Delete(Guid adId, Guid userId)
        {
            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == adId).SingleOrDefault();

            if (currentAd.UserId != userId)
                return null;

            await _dataProvider.Delete(currentAd);

            return new OkResult();
        }

        public AdDto GetAd(Guid adId)
        {
            var ad = _dataProvider.Get<AdDb>(i => i.Id == adId)
                .Select(i => new AdDto
                {
                    Id = i.Id,
                    Text = i.Text,
                    Title = i.Title,
                    CreatedDate = i.CreatedDate,
                    UserId = i.UserId,
                    Ratings = i.Ratings,
                    UserName = i.User.Name
                }).SingleOrDefault();
            
            return ad;
        }

        public List<AdDto> GetAds()
        {
            var ads = _dataProvider.Get<AdDb>(i => i.Id != null)
                .Select(i => new AdDto
                {
                    Id = i.Id,
                    Text = i.Text,
                    Title = i.Title,
                    CreatedDate = i.CreatedDate,
                    UserId = i.UserId,
                    Ratings = i.Ratings,
                    UserName = i.User.Name
                }).OrderByDescending(i => i.CreatedDate).ToList();

            return ads;
        }
    }
}