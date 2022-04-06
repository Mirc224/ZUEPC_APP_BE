using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Queries.Publications.Details.BaseHandlers;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQueryHandler :
	EPCPublicationDetailsQueryHandlerBase,
	IRequestHandler<GetAllPublicationDetailsQuery, GetAllPublicationDetailsQueryResponse>
{
	public GetAllPublicationDetailsQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) { }

	public async Task<GetAllPublicationDetailsQueryResponse> Handle(GetAllPublicationDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllPublicationsQueryResponse response = await _mediator.Send(new GetAllPublicationsQuery() 
		{ 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<Publication> publicationDomains = response.Data;
		List<PublicationDetails> result = new();
		foreach (Publication publication in publicationDomains.OrEmptyIfNull())
		{
			PublicationDetails PublicationPreview = await ProcessPublicationDetails(publication);
			if (PublicationPreview != null)
			{
				result.Add(PublicationPreview);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllPublicationDetailsQueryResponse, PublicationDetails, PublicationFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
