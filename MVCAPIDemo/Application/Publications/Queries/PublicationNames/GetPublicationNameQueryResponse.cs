using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQueryResponse : ResponseBase
{
	public PublicationName? PublicationName { get; set; }
}