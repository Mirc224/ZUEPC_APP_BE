using AutoMapper;
using MediatR;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQueryHandler:
	GetModelsWithFiltersPagedQueryHandlerBase<IPublicationData, Publication, PublicationModel, PublicationFilter>,
	IRequestHandler<GetAllPublicationsQuery, GetAllPublicationsQueryResponse>
{
	public GetAllPublicationsQueryHandler(IMapper mapper, IPublicationData repository)
	: base(mapper, repository) { }

	public async Task<GetAllPublicationsQueryResponse> Handle(GetAllPublicationsQuery request, CancellationToken cancellationToken)
	{
		if (request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllPublicationsQuery, GetAllPublicationsQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllPublicationsQuery, GetAllPublicationsQueryResponse>(request);
	}
}
