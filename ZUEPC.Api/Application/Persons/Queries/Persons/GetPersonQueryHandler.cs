using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetPersonQueryHandler :
	GetSimpleModelQueryHandlerBase<IPersonData, Person, PersonModel, long>,
	IRequestHandler<GetPersonQuery, GetPersonQueryResponse>
{
	public GetPersonQueryHandler(IMapper mapper, IPersonData repository)
	:base(mapper, repository) { }

	public async Task<GetPersonQueryResponse> Handle(GetPersonQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPersonQuery, GetPersonQueryResponse>(request);
	}
}
