using ACA.Application.Commads.Create.CreateProcess;
using ACA.Application.Commads.Delete.DeleteProcess;
using ACA.Application.Commads.Update.UpdateProcess;
using ACA.Application.Queries.Get.GetProcess;
using ACA.GrpcProtos;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

public class ProcessService : ProcessServiceWeb.ProcessServiceWebBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProcessService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<ProcessProto> CreateProcess(Process request, ServerCallContext context)
    {
        var command = new CreateProcessCommand(
            request.ProcessName,
            request.ProductName);

        var result = await _mediator.Send(command);
        return _mapper.Map<ProcessProto>(result);
    }

    public override async Task<ProcessProto> GetProcess(ProcesssGetRequest request, ServerCallContext context)
    {
        var query = new GetProcessByIdQuery(Guid.Parse(request.Id));
        var result = await _mediator.Send(query);
        return _mapper.Map<ProcessProto>(result);
    }

    public override async Task<Processes> GetAllProcesses(Empty request, ServerCallContext context)
    {
        // Implement GetAllProcessesQuery if needed
        throw new RpcException(new Status(StatusCode.Unimplemented, "Method not implemented."));
    }

    public override async Task<Empty> UpdateProcess(ProcessProto request, ServerCallContext context)
    {
        var command = new UpdateProcessCommand(_mapper.Map<ACA.Domain.Entities.Processes.Process>(request));
        await _mediator.Send(command);
        return new Empty();
    }

    public override async Task<Empty> DeleteProcess(ProcessDeleteRequest request, ServerCallContext context)
    {
        var command = new DeleteProcessCommand(Guid.Parse(request.Id));
        await _mediator.Send(command);
        return new Empty();
    }
}


