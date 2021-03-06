using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQueryHandler :
	GetSimpleModelQueryHandlerBase<IPublicationNameData, PublicationName, PublicationNameModel, long>,
	IRequestHandler<GetPublicationNameQuery, GetPublicationNameQueryResponse>
{
	public GetPublicationNameQueryHandler(IMapper mapper, IPublicationNameData repository)
	: base(mapper, repository) { }
	
	public async Task<GetPublicationNameQueryResponse> Handle(GetPublicationNameQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationNameQuery, GetPublicationNameQueryResponse>(request);
	}
}
