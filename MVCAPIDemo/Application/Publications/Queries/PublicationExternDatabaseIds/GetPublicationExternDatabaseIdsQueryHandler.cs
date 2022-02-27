using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdsQueryHandler : IRequestHandler<GetPublicationExternDatabaseIdsQuery, GetPublicationExternDatabaseIdsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public GetPublicationExternDatabaseIdsQueryHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetPublicationExternDatabaseIdsQueryResponse> Handle(GetPublicationExternDatabaseIdsQuery request, CancellationToken cancellationToken)
	{
		var queryResult = await _repository.GetPublicationExternDbIdsByPublicationIdAsync(request.PublicationId);
		var externDbIds = _mapper.Map<List<PublicationExternDatabaseId>>(queryResult);

		return new() { Success = true, PublicationExternDatabaseIds = externDbIds};
	}
}
