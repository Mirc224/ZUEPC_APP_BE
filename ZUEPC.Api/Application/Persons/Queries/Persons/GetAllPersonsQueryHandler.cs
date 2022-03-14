using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQueryHandler :
	GetModelsPagedQueryHandlerBase<IPersonData, Person, PersonModel>,
	IRequestHandler<GetAllPersonsQuery, GetAllPersonsQueryResponse>
{
	public GetAllPersonsQueryHandler(IMapper mapper, IPersonData repository)
		: base(mapper, repository) { }
	public async Task<GetAllPersonsQueryResponse> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetAllPersonsQuery, GetAllPersonsQueryResponse>(request);
	}
}
