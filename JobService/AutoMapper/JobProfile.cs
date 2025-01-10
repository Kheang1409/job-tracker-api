using AutoMapper;
using JobService.DTOs;
using JobService.Models;

namespace JobService.AutoMapper
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
        }
    }
}
