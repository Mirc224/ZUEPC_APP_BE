﻿using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.Publications.Previews.BaseHandlers;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQueryHandler :
	EPCPublicationPreviewQueryHandlerBase,
	IRequestHandler<GetAllPublicationPreviewsQuery, GetAllPublicationPreviewsQueryResponse>
{
	public GetAllPublicationPreviewsQueryHandler(IMapper mapper, IMediator mediator) 
		: base(mapper, mediator) { }
	
	public async Task<GetAllPublicationPreviewsQueryResponse> Handle(GetAllPublicationPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllPublicationsQueryResponse response = await _mediator.Send(new GetAllPublicationsQuery() { PaginationFilter = request.PaginationFilter });

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<Publication> publicationDomains = response.Data;
		List<PublicationPreview> result = new();
		foreach (Publication Publication in publicationDomains.OrEmptyIfNull())
		{
			PublicationPreview PublicationPreview = await ProcessPublicationPreview(Publication);
			if (PublicationPreview != null)
			{
				result.Add(PublicationPreview);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllPublicationPreviewsQueryResponse, GetAllPublicationPreviewsQuery, PublicationPreview>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}
}