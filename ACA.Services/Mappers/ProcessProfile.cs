using AutoMapper;
using ACA.Domain.Entities.Processes; // Asegúrate de que este namespace sea correcto
using ACA.GrpcProtos;

namespace ACA.MappingProfiles
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            // Mapeo de la entidad Process a ProcessProto
            CreateMap<Process, ProcessProto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.ProcessName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));
            //.ForMember(dest => dest.ProcessesOperators, opt => opt.MapFrom(src => src.ProcessesOperators)) // Mapeo de la colección
            // .ForMember(dest => dest.ProcessesSupervisors, opt => opt.MapFrom(src => src.ProcessesSupervisors)); // Mapeo de la colección

            // Mapeo de ProcessProto a la entidad Process
            CreateMap<ProcessProto, Process>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar si se genera automáticamente
                .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.ProcessName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));
            // .ForMember(dest => dest.ProcessesOperators, opt => opt.Ignore()) // Ignorar si se maneja de otra forma
            // .ForMember(dest => dest.ProcessesSupervisors, opt => opt.Ignore()); // Ignorar si se maneja de otra forma
        }
    }
}

