using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdQueryHandler :
	GetSimpleModelQueryHandlerBase<IPersonExternDatabaseIdData, PersonExternDatabaseId, PersonExternDatabaseIdModel, long>,
	IRequestHandler<GetPersonExternDatabaseIdQuery, GetPersonExternDatabaseIdQueryResponse>
{
	public GetPersonExternDatabaseIdQueryHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	: base(mapper, repository) { }
	
	public async Task<GetPersonExternDatabaseIdQueryResponse> Handle(GetPersonExternDatabaseIdQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPersonExternDatabaseIdQuery, GetPersonExternDatabaseIdQueryResponse>(request);
	}
}
