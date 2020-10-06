using System;
using System.Collections.Generic;

namespace AdsApp.Models
{
    public class UserDb
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ICollection<AdDb> Ads { get; set; }
        public ICollection<RatingDb> Ratings { get; set; }
    }
}