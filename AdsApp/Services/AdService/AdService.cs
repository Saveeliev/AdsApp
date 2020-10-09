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
    }
}