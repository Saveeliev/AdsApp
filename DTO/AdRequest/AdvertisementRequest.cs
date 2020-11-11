using Microsoft.AspNetCore.Http;
using System;

namespace DTO.AdRequest
{
    public class AdvertisementRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; }
    }
}