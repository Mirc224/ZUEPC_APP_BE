﻿using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Queries.Publications.Details.BaseHandlers;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQueryHandler :
	EPCPublicationDetailsQueryHandlerBase,
	IRequestHandler<GetAllPublicationDetailsQuery, GetAllPublicationDetailsQueryResponse>
{
	public GetAllPublicationDetailsQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) { }

	public async Task<GetAllPublicationDetailsQueryResponse> Handle(GetAllPublicationDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllPublicationsQueryResponse response = await _mediator.Send(new GetAllPublicationsQuery() { PaginationFilter = request.PaginationFilter });

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
		return PaginationHelper.ProcessResponse<GetAllPublicationDetailsQueryResponse, GetAllPublicationDetailsQuery, PublicationDetails>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}
}