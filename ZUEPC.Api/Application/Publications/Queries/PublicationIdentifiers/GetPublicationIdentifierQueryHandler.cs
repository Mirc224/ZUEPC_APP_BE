using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQueryHandler :
	GetSimpleModelQueryHandlerBase<IPublicationIdentifierData, PublicationIdentifier, PublicationIdentifierModel, long>,
	IRequestHandler<GetPublicationIdentifierQuery, GetPublicationIdentifierQueryResponse>
{
	public GetPublicationIdentifierQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	: base(mapper, repository) { }
	public async Task<GetPublicationIdentifierQueryResponse> Handle(GetPublicationIdentifierQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationIdentifierQuery, GetPublicationIdentifierQueryResponse>(request);
	}
}
