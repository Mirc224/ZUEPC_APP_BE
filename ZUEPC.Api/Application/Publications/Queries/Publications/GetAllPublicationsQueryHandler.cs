using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQueryHandler:
	GetAllPagedSimpleModelQueryHandlerBase<Publication, PublicationModel>,
	IRequestHandler<GetAllPublicationsQuery, GetAllPublicationsQueryResponse>
{
	public GetAllPublicationsQueryHandler(IMapper mapper, IPublicationData repository)
	: base(mapper, repository) { }

	public async Task<GetAllPublicationsQueryResponse> Handle(GetAllPublicationsQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetAllPublicationsQuery, GetAllPublicationsQueryResponse>(request);
	}
}
