﻿using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Users.Queries.Users;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQueryHandler :
	GetModelsWithFiltersPagedQueryHandlerBase<IPersonData, Person, PersonModel, PersonFilter>,
	IRequestHandler<GetAllPersonsQuery, GetAllPersonsQueryResponse>
{
	public GetAllPersonsQueryHandler(IMapper mapper, IPersonData repository)
		: base(mapper, repository) { }
	public async Task<GetAllPersonsQueryResponse> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
	{
		if(request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllPersonsQuery, GetAllPersonsQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllPersonsQuery, GetAllPersonsQueryResponse>(request);
	}
}
