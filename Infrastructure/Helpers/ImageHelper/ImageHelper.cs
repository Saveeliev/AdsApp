using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly StaticFilesOptions _options;

        public ImageHelper(IOptions<StaticFilesOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> UploadImageAndGetName(IFormFile image)
        {
            var ext = Path.GetExtension(image.FileName);

            var relativePath = Path.Combine(_options.RelativeImagePath, $"{Guid.NewGuid()}{ext}");

            var absolutePath = Path.Combine(_options.BasePath, relativePath);

            using var fs = new FileStream(absolutePath, FileMode.Create);

            await image.CopyToAsync(fs);

            return relativePath;
        }

        public byte[] GetImage(string fileName, string width, string height)
        {
            var absolutePath = Path.Combine(_options.BasePath, _options.RelativeImagePath, fileName);

            if(!String.IsNullOrEmpty(width) || !String.IsNullOrEmpty(height))
            {
                using var image = Image.Load(absolutePath);

                image.Mutate(x => x.Resize(Convert.ToInt32(width), Convert.ToInt32(height)));

                using var memoryStream = new MemoryStream();

                image.SaveAsBmp(memoryStream);

                return memoryStream.ToArray();
            }

            return File.ReadAllBytes(absolutePath);
        }
    }
}