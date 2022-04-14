using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDbIdsByPublicationIdInSetQuery : IRequest<GetAllPublicationExternDbIdsByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}

