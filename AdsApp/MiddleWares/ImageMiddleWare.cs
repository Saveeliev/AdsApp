using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace AdsApp.MiddleWares
{
    public class ImageMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IImageHelper _imageHelper;

        public ImageMiddleWare(RequestDelegate next, IImageHelper imageHelper)
        {
            _next = next;
            _imageHelper = imageHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if(CheckImagePath(path))
            {
                var fileName = path.ToString().Split("/").Last();

                var width = context.Request.Query["width"];
                var height = context.Request.Query["height"];

                var bytes = _imageHelper.GetImage(fileName, width, height);

                await context.Response.BodyWriter.WriteAsync(bytes);
            }

            await _next.Invoke(context);
        }

        private bool CheckImagePath(string path)
        {
            return path.TrimStart('/').Split('/').FirstOrDefault() == "Image";
        }
    }
}