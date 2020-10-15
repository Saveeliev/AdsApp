using DataBase.Models;
using DTO;
using DTO.AdRequest;
using Infrastructure.Options;
using Infrastructure.Services.DataProvider;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task AddAdvertisement(AddAdvertisementRequest ad, Guid userId)
        {
            if (ad is null)
            {
                throw new ArgumentNullException(nameof(ad));
            }

            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.Serializable);

            var ads = _dataProvider.Get<AdDb>(i => i.UserId == userId).ToList();

            if (ads.Count >= UserOptions.AdCountLimit)
            {
                throw new Exception($"You cannot add more than {UserOptions.AdCountLimit} ads");
            }

            byte[] image = null;

            if (ad.Image != null)
            {
                using var binaryReader = new BinaryReader(ad.Image.OpenReadStream());
                image = binaryReader.ReadBytes((int)ad.Image.Length);
            }

            var currentAd = new AdDb { 

                CreatedDate = DateTime.Now, 
                Image = image, 
                Title = ad.Title, 
                Text = ad.Text, 
                UserId = userId 
            };

            await _dataProvider.Insert(currentAd);

            transaction.Commit();
        }

        public async Task UpdateAdvertisement(Guid adId, string adText, string adTitle, Guid userId)
        {
            if (adText is null || adTitle is null)
            {
                throw new ArgumentNullException();
            }

            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == adId).SingleOrDefault();

            if (currentAd.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }    

            currentAd.Text = adText;
            currentAd.Title = adTitle;

            await _dataProvider.Update(currentAd);

            transaction.Commit();
        }

        public async Task Like(Guid adId, Guid userId)
        {
            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var isLiked = _dataProvider.Get<RatingDb>(i => i.AdId == adId && i.UserId == userId).SingleOrDefault();

            if(isLiked == null)
            {
                var like = new RatingDb { 

                    AdId = adId, 
                    UserId = userId,
                    IsLiked = true
                };

                await _dataProvider.Insert(like);
            }
            else
            {
                if (isLiked.IsLiked == true)
                {
                    await _dataProvider.Delete(isLiked);
                }
                else
                {
                    isLiked.IsLiked = true;
                    await _dataProvider.Update(isLiked);
                }
            }

            transaction.Commit();
        }

        public async Task DisLike(Guid adId, Guid userId)
        {
            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var isLiked = _dataProvider.Get<RatingDb>(i => i.AdId == adId && i.UserId == userId).SingleOrDefault();

            if (isLiked == null)
            {
                var disLike = new RatingDb { 
                    AdId = adId, 
                    UserId = userId, 
                    IsLiked = false 
                };

                await _dataProvider.Insert(disLike);
            }
            else
            {
                if (isLiked.IsLiked == false)
                {
                    await _dataProvider.Delete(isLiked);
                }
                else
                {
                    isLiked.IsLiked = false;
                    await _dataProvider.Update(isLiked);
                }
            }

            transaction.Commit();
        }

        public async Task Delete(Guid adId, Guid userId)
        {
            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == adId).SingleOrDefault();

            if (currentAd.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            await _dataProvider.Delete(currentAd);

            transaction.Commit();
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