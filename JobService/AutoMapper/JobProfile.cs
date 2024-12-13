using AutoMapper;
using JobService.DTOs;
using JobService.Models;

namespace JobService.AutoMapper
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            // Map between Job and JobDto
            CreateMap<Job, JobDto>().ReverseMap();

            // Map between Job and CreateJobDto
            CreateMap<Job, CreateJobDto>().ReverseMap();
        }
    }
}
