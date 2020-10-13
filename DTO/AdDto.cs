using DataBase.Models;
using System;
using System.Collections.Generic;

namespace DTO
{
    public class AdDto
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public ICollection<RatingDb> Ratings { get; set; }
        public string UserName { get; set; }
    }
}