using MediatR;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries;

public class GetAllPublicationAuthorsByPublicationIdInSetQuery : IRequest<GetAllPublicationAuthorsByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}
