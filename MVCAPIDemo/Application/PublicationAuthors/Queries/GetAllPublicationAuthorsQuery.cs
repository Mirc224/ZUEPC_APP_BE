using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetAllPublicationAuthorsQuery : IRequest<GetAllPublicationAuthorsQueryResponse>
{
	public long PublicationId { get; set; }
}
