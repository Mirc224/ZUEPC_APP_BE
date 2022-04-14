using MediatR;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries.Details;

public class GetAllPublicationAuthorsDetailsByPublicationIdInSetQuery : IRequest<GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}
