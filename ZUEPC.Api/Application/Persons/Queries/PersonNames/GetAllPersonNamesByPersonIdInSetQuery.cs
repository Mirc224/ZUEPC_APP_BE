using MediatR;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesByPersonIdInSetQuery : IRequest<GetAllPersonNamesByPersonIdInSetQueryResponse>
{
	public IEnumerable<long> PersonIds { get; set; }
}
