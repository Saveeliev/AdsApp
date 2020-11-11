using DTO;
using DTO.AdRequest;

namespace AdsApp.Models
{
    public class IndexViewModel
    {
        public AdDto[] Ads { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SearchRequest SearchRequest { get; set; }
    }
}