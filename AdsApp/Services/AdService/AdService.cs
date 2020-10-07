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
            var currentUser = _dataProvider.Get<UserDb>(i => i.Login == ad.UserName).SingleOrDefault();
            var currentAd = new AdDb { CreatedDate = DateTime.Now, Text = ad.Text, UserId = currentUser.Id };
            await _dataProvider.Insert(currentAd);
        }

        public List<AdDto> GetAds()
        {
            var ads = _dataProvider.Get<AdDb>(i => i.Id != null).Select(
                j => new AdDto
                {
                    Id = j.Id,
                    UserName = j.User.Login,
                    CreatedDate = j.CreatedDate,
                    Text = j.Text
                }).ToList();
            return ads;
        }
    }
}