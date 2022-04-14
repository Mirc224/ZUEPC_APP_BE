using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersByPublicationIdInSetQuery : IRequest<GetAllPublicationIdentifiersByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}
