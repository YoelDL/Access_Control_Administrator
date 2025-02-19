using MediatR;

namespace ACA.Application.Abstract
{
    public interface IQueryHandler<TQuery, TResponse> 
        : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {

    }
}
