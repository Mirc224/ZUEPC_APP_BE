using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationExternDbIdsInSetQueryHandler : IRequestHandler<GetAllPublicationExternDbIdsInSetQuery, GetAllPublicationExternDbIdsInSetQueryResponse>
{
	private readonly IPublicationExternDatabaseIdData _repository;
	private readonly IMapper _mapper;

	public GetAllPublicationExternDbIdsInSetQueryHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<GetAllPublicationExternDbIdsInSetQueryResponse> Handle(GetAllPublicationExternDbIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationExternDatabaseIdModel> externIds = await _repository.GetAllPublicationExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = false };
		}

		List<PublicationExternDatabaseId>? mapedResult = _mapper.Map<List<PublicationExternDatabaseId>>(externIds);
		return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = true, Data = mapedResult };
	}
}
