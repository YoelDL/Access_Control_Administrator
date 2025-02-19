using AutoMapper;
using ACA.GrpcProtos;
using Supervisor = ACA.Domain.Entities.Workers.Supervisor;


namespace ACA.GrpcService.Mappers
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
                .ForMember(dest => dest.Operators, opt => opt.MapFrom(src => src.Operators)) // Mapeo de la coleccion
                .ForMember(dest => dest.ProcessesSupervisors, opt => opt.MapFrom(src => src.Processes_Supervisors)); //Mapeo de la coleccion 

            // Mapeo de SupervisorProto a la entidad Supervisor
            CreateMap<SupervisorProto, Supervisor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Supervisor_Name, opt => opt.MapFrom(src => src.SupervisorName))
                .ForMember(dest => dest.CI, opt => opt.MapFrom(src => src.Ci))
                .ForMember(dest => dest.Operators, opt => opt.MapFrom(src => src.Operators)) //Mapeo de la coleccion
                .ForMember(dest => dest.Processes_Supervisors, opt => opt.MapFrom(src => src.ProcessesSupervisors)); //Mapeo de la coleccion 
            

        }
    }
}
