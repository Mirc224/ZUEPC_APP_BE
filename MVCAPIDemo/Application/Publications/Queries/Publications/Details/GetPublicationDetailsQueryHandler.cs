using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Queries.Publications.Details.BaseHandlers;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQueryHandler :
	EPCPublicationDetailsQueryHandlerBase,
	IRequestHandler<GetPublicationDetailsQuery, GetPublicationDetailsQueryResponse>
{
	public GetPublicationDetailsQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) {}
	public async Task<GetPublicationDetailsQueryResponse> Handle(GetPublicationDetailsQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.Id;
		Publication? publication = (await _mediator.Send(new GetPublicationQuery()
		{
			Id = publicationId
		})).Data;

		if (publication is null)
		{
			return new() { Success = false };
		}
		PublicationDetails result = await ProcessPublicationDetails(publication);

		return new() { Success = true, Data = result };
	}
}
