using ACA.Application.Commads.Delete.DeleteSupervisor;
using ACA.Application.Commads.Update.UpdateSupervisor;
using ACA.Application.Commands.Create.CreateSupervisor;
using ACA.Application.Queries.Get.GetSupervisor;
using ACA.GrpcProtos;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

public class SupervisorService : SupervisorServiceWeb.SupervisorServiceWebBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SupervisorService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<SupervisorProto> CreateSupervisor(Supervisor request, ServerCallContext context)
    {
        var command = new CreateSupervisorCommand(
            request.SupervisorName,
            request.Ci,
            (ACA.Domain.Entities.Types.SchoolLevel)request.SchoolLevel);

        var result = await _mediator.Send(command);
        return _mapper.Map<SupervisorProto>(result);
    }

    public override async Task<SupervisorProto> GetSupervisor(SupervisorGetRequest request, ServerCallContext context)
    {
        var query = new GetSupervisorByIdQuery(Guid.Parse(request.Id));
        var result = await _mediator.Send(query);
        return _mapper.Map<SupervisorProto>(result);
    }

    public override async Task<Supervisors> GetAllSupervisors(Empty request, ServerCallContext context)
    {
        // Implement GetAllSupervisorsQuery if needed
        throw new RpcException(new Status(StatusCode.Unimplemented, "Method not implemented."));
    }

    public override async Task<Empty> UpdateSupervisor(SupervisorProto request, ServerCallContext context)
    {
        var command = new UpdateSupervisorCommand(_mapper.Map<ACA.Domain.Entities.Workers.Supervisor>(request));
        await _mediator.Send(command);
        return new Empty();
    }

    public override async Task<Empty> DeleteSupervisor(SupervisorDeleteRequest request, ServerCallContext context)
    {
        var command = new DeleteSupervisorCommand(Guid.Parse(request.Id));
        await _mediator.Send(command);
        return new Empty();
    }
}

