using AutoMapper;
using DTO;
using DTO.AdRequest;

namespace Infrastructure.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AdDto, AdvertisementRequest>();
        }
    }
}
