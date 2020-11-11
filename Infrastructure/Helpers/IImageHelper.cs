using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAndGetName(IFormFile image);
        byte[] GetImage(string fileName, string width, string height);
    }
}