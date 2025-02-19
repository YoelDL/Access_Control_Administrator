using AutoMapper;
using ACA.GrpcProtos;
using ACA.Domain.Entities.Workers;

namespace ACA.MappingProfiles
{
    public class OperatorProfile : Profile
    {
        public OperatorProfile()
        {
            // Mapeo de la entidad Operator a OperatorProto
            CreateMap<Operator, OperatorProto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OperatorName, opt => opt.MapFrom(src => src.Operator_Name))
                .ForMember(dest => dest.Ci, opt => opt.MapFrom(src => src.))
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => (SchoolLevelProto)src.schoolLevel)); // Enum
            // .ForMember(dest => dest.ProcessesOperators, opt => opt.MapFrom(src => src.ProcessesOperators)); // Mapeo de la colección

            // Mapeo de OperatorProto a la entidad Operator
            CreateMap<OperatorProto, Operator>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar si se genera automáticamente
                .ForMember(dest => dest.OperatorName, opt => opt.MapFrom(src => src.OperatorName))
                .ForMember(dest => dest.Ci, opt => opt.MapFrom(src => src.Ci))
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => (SchoolLevelProto)src.SchoolLevel)); // Enum
                                                                                                               // .ForMember(dest => dest.ProcessesOperators, opt => opt.Ignore()); // Ignorar si se maneja de otra forma
        }
    }
}

