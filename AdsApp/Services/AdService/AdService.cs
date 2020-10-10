using AdsApp.Models;
using AdsApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public class AdService : IAdService
    {
        private readonly IDataProvider _dataProvider;

        public AdService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task AddAdvertisement(AdDto ad)
        {
            var currentAd = new AdDb { CreatedDate = DateTime.Now, Text = ad.Text, UserId = ad.UserId };

            await _dataProvider.Insert(currentAd);
        }

        public async Task UpdateAdvertisement(AdDto ad)
        {
            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == ad.Id).SingleOrDefault();
            currentAd.Text = ad.Text;

            await _dataProvider.Update(currentAd);
        }

        public AdDto GetAd(Guid adId)
        {
            var ad = _dataProvider.Get<AdDb>(i => i.Id == adId)
                .Select(i => new AdDto
                {
                    Id = i.Id,
                    Text = i.Text,
                    CreatedDate = i.CreatedDate
                }).SingleOrDefault();

            return ad;
        }

        public List<AdDto> GetAllAds()
        {
            var ads = _dataProvider.Get<AdDb>(i => i.Id != null)
                .Select(i => new AdDto
                {
                    Id = i.Id,
                    Text = i.Text,
                    CreatedDate = i.CreatedDate,
                    Image = i.Image,
                    UserId = i.UserId,
                    Number = i.Number
                }).OrderByDescending(i => i.CreatedDate).ToList();

            return ads;
        }
    }
}