using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQueryHandler :
	GetSimpleModelQueryHandlerBase<IPublicationActivityData, PublicationActivity, PublicationActivityModel, long>,
	IRequestHandler<GetPublicationActivityQuery, GetPublicationActivityQueryResponse>
{
	public GetPublicationActivityQueryHandler(IMapper mapper, IPublicationActivityData repository)
	: base(mapper, repository) { }
	public async Task<GetPublicationActivityQueryResponse> Handle(GetPublicationActivityQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationActivityQuery, GetPublicationActivityQueryResponse>(request);
	}
}
