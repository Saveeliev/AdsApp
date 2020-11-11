using System;

namespace AdsApp.Models
{
    public class PageViewModel
    {
        public int CurrentPageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int adsCount, int currentPageNumber, int pageSize)
        {
            CurrentPageNumber = currentPageNumber;

            TotalPages = (int)Math.Ceiling(adsCount / (double)pageSize);
        }
    }
}