using AutoMapper;
using MediatR;
using Grpc.Core;
using ACA.GrpcProtos;
using ACA.Application.Commands.Create.CreateOperator;
using ACA.Application.Queries.Get.GetOperator;
using ACA.Application.Commads.Update.UpdateOperator;
using ACA.Application.Commands.Delete.DeleteOperator;
using Google.Protobuf.WellKnownTypes;

namespace ACA.Services
{
    public class OperatorService : OperatorServiceWeb.OperatorServiceWebBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OperatorService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<OperatorProto> CreateOperator(Operator request, ServerCallContext context)
        {
            var command = new CreateOperatorCommand(
                request.OperatorName,
                request.Ci,
                (Domain.Entities.Types.SchoolLevel)request.SchoolLevel,
                null); // AsignedSupervisorId is null initially

            var result = await _mediator.Send(command);
            return _mapper.Map<OperatorProto>(result);
        }

        public override async Task<OperatorProto> GetOperator(OperatorGetRequest request, ServerCallContext context)
        {
            var query = new GetOperatorByIdQuery(Guid.Parse(request.Id));
            var result = await _mediator.Send(query);
            return _mapper.Map<OperatorProto>(result);
        }

        public override async Task<Operators> GetAllOperators(Empty request, ServerCallContext context)
        {
            // Implement GetAllOperatorsQuery if needed
            throw new RpcException(new Status(StatusCode.Unimplemented, "Method not implemented."));
        }

        public override async Task<Empty> UpdateOperator(OperatorProto request, ServerCallContext context)
        {
            var command = new UpdateOperatorCommand(_mapper.Map<Domain.Entities.Workers.Operator>(request));
            await _mediator.Send(command);
            return new Empty();
        }

        public override async Task<Empty> DeleteOperator(OperatorDeleteRequest request, ServerCallContext context)
        {
            var command = new DeleteOperatorCommand(Guid.Parse(request.Id));
            await _mediator.Send(command);
            return new Empty();
        }
    }
}
