using AutoMapper;
using DataBase.Models;
using DTO;
using DTO.AdRequest;

namespace Infrastructure.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AdDb, AdDto>();
        }
    }
}