using AutoMapper;
using MediatR;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetPublicationWithDetailsQueryHandler : IRequestHandler<GetPublicationWithDetailsQuery, GetPublicationWithDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationWithDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public Task<GetPublicationWithDetailsQueryResponse> Handle(GetPublicationWithDetailsQuery request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
