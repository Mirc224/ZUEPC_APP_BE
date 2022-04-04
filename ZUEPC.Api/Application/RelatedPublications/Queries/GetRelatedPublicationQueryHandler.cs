using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQueryHandler : 
	GetSimpleModelQueryHandlerBase<IRelatedPublicationData, RelatedPublication, RelatedPublicationModel, long>,
	IRequestHandler<GetRelatedPublicationQuery, GetRelatedPublicationQueryResponse>
{

	public GetRelatedPublicationQueryHandler(IMapper mapper, IRelatedPublicationData repository)
		: base(mapper, repository) { }
	public async Task<GetRelatedPublicationQueryResponse> Handle(GetRelatedPublicationQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetRelatedPublicationQuery, GetRelatedPublicationQueryResponse>(request);
	}
}
