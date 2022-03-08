using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQuery : IRequest<GetPublicationIdentifierQueryResponse>
{
	public long PublicationIdentifierRecordId { get; set; }
}
