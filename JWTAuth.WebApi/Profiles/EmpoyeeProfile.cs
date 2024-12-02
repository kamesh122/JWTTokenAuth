using AutoMapper;
using JWTAuth.WebApi.DTOs;
using JWTAuth.WebApi.Models;

namespace JWTAuth.WebApi.Profiles
{
    public class EmpoyeeProfile : Profile
    {
        public EmpoyeeProfile()
        {
            // CreateMap<EmpolyeeDTO, Employee>()
            // .ForMember(dest => dest.EmployeeID, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Name))
            //.ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Title));

            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeDTO>();

        }
    }
}
