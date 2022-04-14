using MediatR;

namespace ZUEPC.Api.Application.RelatedPublications.Queries;

public class GetAllRelatedPublicationByPublicationIdInSetQuery : 
	IRequest<GetAllRelatedPublicationByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}