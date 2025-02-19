using AutoMapper;
using ACA.GrpcProtos;
using Operator = ACA.Domain.Entities.Workers.Operator;

namespace ACA.GrpcService.Mappers
{
    public class OperatorProfile : Profile
    {
        public OperatorProfile()
        {
            // Mapeo de la entidad Operator a OperatorProto
            CreateMap<Operator, OperatorProto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OperatorName, opt => opt.MapFrom(src => src.Operator_Name))
                .ForMember(dest => dest.Ci, opt => opt.MapFrom(src => src.CI))
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.schoolLevel))
                .ForMember(dest => dest.ProcessList, opt => opt.MapFrom(src => src.Processes_Operators)) //Mapeo de la coleccion 
                .ForMember(dest => dest.SupervisorAsigned, opt => opt.MapFrom(src => src.Asigned_Supervisor)); //Mapeo de la relacion 

                

            // Mapeo de OperatorProto a la entidad Operator
            CreateMap<OperatorProto, Operator>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Operator_Name, opt => opt.MapFrom(src => src.OperatorName))
                .ForMember(dest => dest.CI, opt => opt.MapFrom(src => src.Ci))
                .ForMember(dest => dest.schoolLevel, opt => opt.MapFrom(src => src.SchoolLevel))
                .ForMember(dest => dest.Processes_Operators, opt => opt.MapFrom(src => src.ProcessList)) // Mapeo de la coleccion 
                .ForMember(dest => dest.Asigned_Supervisor, opt => opt.MapFrom(src => src.SupervisorAsigned)); //Mapeo de la relacion 


        }
    }
}

