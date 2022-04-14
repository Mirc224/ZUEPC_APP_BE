using MediatR;

namespace ZUEPC.Api.Application.Persons.Queries.Persons;

public class GetAllPersonsWithIdInSetQuery : IRequest<GetAllPersonsWithIdInSetQueryResponse>
{
	public IEnumerable<long> PersonIds { get; set; }
}
