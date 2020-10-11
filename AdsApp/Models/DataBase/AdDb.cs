using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace AdsApp.Models
{
    public class AdDb
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid UserId { get; set; }
        public UserDb User { get; set; }

        public ICollection<RatingDb> Ratings { get; set; }
    }
}