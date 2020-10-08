using AdsApp.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IAdService
    {
        Task AddAdvertisement(AdDto ad);
    }
}