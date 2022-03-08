using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQuery : IRequest<GetPublicationNameQueryResponse>
{
	public long PublicatioNameRecordId { get; set; }
}
