using System;

namespace DTO.AdRequest
{
    public class SearchRequest
    {
        public string SearchText { get; set; }
        public DateTime? SearchDate { get; set; }
        public bool? SortRating { get; set; }
        public bool? SortDate { get; set; }
        public bool? SortNumber { get; set; }
    }
}