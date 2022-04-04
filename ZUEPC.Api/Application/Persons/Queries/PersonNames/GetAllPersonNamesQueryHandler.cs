using AutoMapper;
using MediatR;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesQueryHandler :
	GetModelsWithFiltersPagedQueryHandlerBase<IPersonNameData, PersonName, PersonNameModel, PersonNameFilter>,
	IRequestHandler<GetAllPersonNamesQuery, GetAllPersonNamesQueryResponse>
{
	public GetAllPersonNamesQueryHandler(IMapper mapper, IPersonNameData repository) : base(mapper, repository)
	{
	}

	public async Task<GetAllPersonNamesQueryResponse> Handle(GetAllPersonNamesQuery request, CancellationToken cancellationToken)
	{
		if (request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllPersonNamesQuery, GetAllPersonNamesQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllPersonNamesQuery, GetAllPersonNamesQueryResponse>(request);
	}
}
