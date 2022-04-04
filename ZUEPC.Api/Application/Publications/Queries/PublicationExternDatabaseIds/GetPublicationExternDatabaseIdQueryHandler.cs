using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQueryHandler :
	GetSimpleModelQueryHandlerBase<IPublicationExternDatabaseIdData, PublicationExternDatabaseId, PublicationExternDatabaseIdModel, long>,
	IRequestHandler<GetPublicationExternDatabaseIdQuery, GetPublicationExternDatabaseIdQueryResponse>
{

	public GetPublicationExternDatabaseIdQueryHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	: base(mapper, repository) { }
	
	public async Task<GetPublicationExternDatabaseIdQueryResponse> Handle(GetPublicationExternDatabaseIdQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationExternDatabaseIdQuery, GetPublicationExternDatabaseIdQueryResponse>(request);
	}
}
