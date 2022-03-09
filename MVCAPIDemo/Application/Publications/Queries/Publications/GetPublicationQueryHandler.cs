using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publictions;

public class GetPublicationQueryHandler :
	EPCSimpleModelQueryHandlerBase<Publication, PublicationModel>,
	IRequestHandler<GetPublicationQuery, GetPublicationQueryResponse>
{
	public GetPublicationQueryHandler(IMapper mapper, IPublicationData repository)
	: base(mapper, repository) { }

	public async Task<GetPublicationQueryResponse> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationQuery, GetPublicationQueryResponse>(request);
	}
}
