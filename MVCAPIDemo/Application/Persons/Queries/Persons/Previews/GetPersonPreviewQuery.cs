using MediatR;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQuery : IRequest<GetPersonPreviewQueryResponse>
{
	public long PersonId { get; set; }
}
