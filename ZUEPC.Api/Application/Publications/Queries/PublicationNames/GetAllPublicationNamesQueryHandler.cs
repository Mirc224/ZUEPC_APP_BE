using AutoMapper;
using MediatR;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQueryHandler :
	GetModelsPaginatedQueryWithFiltersHandlerBase<IPublicationNameData, PublicationName, PublicationNameModel, PublicationNameFilter>,
	IRequestHandler<GetAllPublicationNamesQuery, GetAllPublicationNamesQueryResponse>
{
	public GetAllPublicationNamesQueryHandler(IMapper mapper, IPublicationNameData repository) : base(mapper, repository)
	{
	}

	public async Task<GetAllPublicationNamesQueryResponse> Handle(GetAllPublicationNamesQuery request, CancellationToken cancellationToken)
	{
		if (request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllPublicationNamesQuery, GetAllPublicationNamesQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllPublicationNamesQuery, GetAllPublicationNamesQueryResponse>(request);
	}
}
