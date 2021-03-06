using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNameQueryHandler :
	GetSimpleModelQueryHandlerBase<IPersonNameData, PersonName, PersonNameModel, long>,
	IRequestHandler<GetPersonNameQuery, GetPersonNameQueryResponse>
{
	public GetPersonNameQueryHandler(IMapper mapper, IPersonNameData repository)
	: base(mapper, repository) { }
	
	public async Task<GetPersonNameQueryResponse> Handle(GetPersonNameQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPersonNameQuery, GetPersonNameQueryResponse>(request);
	}
}
