using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQueryHandler : 
	EPCSimpleModelQueryHandlerBase<RelatedPublication, RelatedPublicationModel>,
	IRequestHandler<GetRelatedPublicationQuery, GetRelatedPublicationQueryResponse>
{

	public GetRelatedPublicationQueryHandler(IMapper mapper, IRelatedPublicationData repository)
		: base(mapper, repository) { }
	public async Task<GetRelatedPublicationQueryResponse> Handle(GetRelatedPublicationQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetRelatedPublicationQuery, GetRelatedPublicationQueryResponse>(request);
	}
}
