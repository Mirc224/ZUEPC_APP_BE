using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetPublicationQueryResponse : ResponseBase
{
	public Publication? Publication { get; set; }
}