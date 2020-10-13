using System;

namespace DataBase.Models
{
    public class RatingDb
    {
        public Guid Id { get; set; }

        public bool IsLiked { get; set; }

        public Guid? UserId { get; set; }
        public UserDb User { get; set; }

        public Guid? AdId { get; set; }
        public AdDb Ad { get; set; }
    }
}