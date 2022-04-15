using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.PublicationAuthors.Queries.Details;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQueryHandler : IRequestHandler<GetPublicationAuthorDetailsQuery, GetPublicationAuthorDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationAuthorDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetPublicationAuthorDetailsQueryResponse> Handle(GetPublicationAuthorDetailsQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;
		GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryResponse? response = await _mediator
			.Send(new GetAllPublicationAuthorsDetailsByPublicationIdInSetQuery() { PublicationIds = new long[] { publicationId } });
		return new() { Success = response.Success, Data = response.Data };
	}
}
