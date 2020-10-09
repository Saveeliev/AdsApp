using System;

namespace AdsApp.Models.ViewModels
{
    public class AdDto
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
    }
}