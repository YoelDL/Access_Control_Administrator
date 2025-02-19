using AutoMapper;
using ACA.GrpcProtos;
using Process = ACA.Domain.Entities.Processes.Process;

namespace ACA.GrpcService.Mappers
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            // Mapeo de la entidad Process a ProcessProto
            CreateMap<Process, ProcessProto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.ProcessName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.OperatorsInProcess, opt => opt.MapFrom(src => src.Processes_Operators)) // Mapeo de la colección
                .ForMember(dest => dest.SupervisorsInProcess, opt => opt.MapFrom(src => src.Processes_Supervisors)); // Mapeo de la colección

            // Mapeo de ProcessProto a la entidad Process
            CreateMap<ProcessProto, Process>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.ProcessName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Processes_Operators, opt => opt.MapFrom(src => src.OperatorsInProcess)) // Mapeo de la colección
                .ForMember(dest => dest.Processes_Supervisors, opt => opt.MapFrom(src => src.SupervisorsInProcess)); // Mapeo de la colección
        }
    }
}

