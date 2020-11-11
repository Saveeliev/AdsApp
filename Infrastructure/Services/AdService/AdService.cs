using DataBase.Models;
using DTO;
using DTO.AdRequest;
using Infrastructure.Helpers;
using Infrastructure.Options;
using Infrastructure.Services.DataProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.AdService
{
    public class AdService : IAdService
    {
        private readonly IDataProvider _dataProvider;
        private readonly IImageHelper _imageHelper;
        private readonly StaticFilesOptions _options;

        public AdService(IDataProvider dataProvider, IImageHelper imageHelper, IOptions<StaticFilesOptions> options)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _imageHelper = imageHelper ?? throw new ArgumentNullException(nameof(imageHelper));
            _options = options.Value;
        }

        public async Task AddAdvertisement(AddAdvertisementRequest request, Guid userId)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.Serializable);

            var ads = _dataProvider.Get<AdDb>(i => i.UserId == userId).ToList();

            if (ads.Count >= UserOptions.AdCountLimit)
            {
                throw new Exception($"You cannot add more than {UserOptions.AdCountLimit} ads");
            }

            string imagePath = null;

            if (request.Image != null)
            {
                imagePath = _imageHelper.UploadImageAndGetName(request.Image).Result;
            }

            var currentAd = new AdDb
            {
                ImagePath = imagePath,
                CreatedDate = DateTime.Now,
                Title = request.Title,
                Text = request.Text,
                UserId = userId
            };

            await _dataProvider.Insert(currentAd);

            transaction.Commit();
        }

        public async Task UpdateAdvertisement(AdvertisementRequest request, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(request.Text) || request.Title is null)
            {
                throw new ArgumentNullException();
            }

            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var currentAd = _dataProvider.Get<AdDb>(i => i.Id == request.Id).SingleOrDefault();

            if (currentAd.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            if(request.Image != null)
            {
                currentAd.ImagePath = _imageHelper.UploadImageAndGetName(request.Image).Result;
            }
            
            currentAd.Text = request.Text;
            currentAd.Title = request.Title;



            await _dataProvider.Update(currentAd);

            transaction.Commit();
        }

        public async Task Like(Guid adId, Guid userId)
        {
            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.RepeatableRead);

            var isLiked = _dataProvider.Get<RatingDb>(i => i.AdId == adId && i.UserId == userId).SingleOrDefault();

            if (isLiked == null)
            {
                var like = new RatingDb
                {

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
                var disLike = new RatingDb
                {
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

            if (currentAd.ImagePath != null)
            {
                var path = Path.Combine(_options.BasePath, currentAd.ImagePath);

                var file = new FileInfo(path);
                file.Delete();
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
                    UserName = i.User.Name,
                    ImagePath = i.ImagePath
                }).SingleOrDefault();

            return ad;
        }

        public async Task<AdDto[]> Search(SearchRequest request)
        {
            var query = _dataProvider.Get<AdDb>();

            if (request.SearchDate.HasValue)
            {
                query = query.Where(i => i.CreatedDate.Date == request.SearchDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(i => i.Text.Contains(request.SearchText) || i.Title.Contains(request.SearchText));
            }

            if (request.SortDate.HasValue)
            {
                if (request.SortDate.Value)
                {
                    query = query.OrderBy(i => i.CreatedDate);
                }
                else
                {
                    query = query.OrderByDescending(i => i.CreatedDate);
                }
            }
            else
            {
                query = query.OrderByDescending(i => i.CreatedDate);
            }

            if (request.SortNumber.HasValue)
            {
                if (request.SortNumber.Value)
                {
                    query = query.OrderBy(i => i.Number);
                }
                else
                {
                    query = query.OrderByDescending(i => i.Number);
                }
            }

            if (request.SortRating.HasValue)
            {
                if (request.SortRating.Value)
                {
                    query = query.OrderBy(i => i.Ratings.Where(i => i.IsLiked).Count() - i.Ratings.Where(i => !i.IsLiked).Count());
                }
                else
                {
                    query = query.OrderByDescending(i => i.Ratings.Where(i => i.IsLiked).Count() - i.Ratings.Where(i => !i.IsLiked).Count());
                }
            }

            return await query.Select(i => new AdDto
            {
                Id = i.Id,
                Text = i.Text,
                Title = i.Title,
                CreatedDate = i.CreatedDate,
                UserId = i.UserId,
                Ratings = i.Ratings,
                UserName = i.User.Name,
                ImagePath = i.ImagePath
            }).ToArrayAsync();
        }
    }
}