using DataBase.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AdDto
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public ICollection<RatingDb> Ratings { get; set; }
        public string UserName { get; set; }
    }
}