using AutoMapper;
using ACA.Domain.Entities;
using ACA.GrpcProtos;
using ACA.Domain.Entities.Types;
using ACA.Domain.Entities.Workers;

namespace ACA.MappingProfiles
{
    public class SupervisorProfile : Profile
    {
        public SupervisorProfile()
        {
            // Mapeo de la entidad Supervisor a SupervisorProto
            CreateMap<Supervisor, SupervisorProto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.SupervisorName, opt => opt.MapFrom(src => src.Supervisor_Name))
                .ForMember(dest => dest.Ci, opt => opt.MapFrom(src => src.CI))
                 .ForMember(dest => dest.schoolLevel, opt => opt.MapFrom(src => (SchoolLevel)src.schoolLevel)); // Enum
            //.ForMember(dest => dest.Operators, opt => opt.MapFrom(src => src.Operators))
            //.ForMember(dest => dest.ProcessesSupervisors, opt => opt.MapFrom(src => src.ProcessesSupervisors));

            // Mapeo de SupervisorProto a la entidad Supervisor
            CreateMap<SupervisorProto, Supervisor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar si se genera automáticamente
                .ForMember(dest => dest.Supervisor_Name, opt => opt.MapFrom(src => src.SupervisorName))
                .ForMember(dest => dest.CI, opt => opt.MapFrom(src => src.Ci))
                  .ForMember(dest => dest.schoolLevel, opt => opt.MapFrom(src => (SchoolLevel)src.schoolLevel)); // Enum
                                                                                                                 // .ForMember(dest => dest.Operators, opt => opt.Ignore()) // Ignorar si se maneja de otra forma
                                                                                                                 // .ForMember(dest => dest.ProcessesSupervisors, opt => opt.Ignore()); // Ignorar si se maneja de otra forma
        }
    }
}
