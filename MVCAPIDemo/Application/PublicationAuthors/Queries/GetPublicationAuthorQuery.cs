using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQuery : IRequest<GetPublicationAuthorQueryResponse>
{
	public long PublicationAuthorRecordId { get; set; }
}
