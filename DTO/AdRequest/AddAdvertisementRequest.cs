using Microsoft.AspNetCore.Http;

namespace DTO.AdRequest
{
    public class AddAdvertisementRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; }
    }
}