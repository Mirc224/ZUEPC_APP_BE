using AutoMapper;
using MediatR;
using System.Collections.Generic;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

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
		var externIds = await _repository.GetAllPublicationExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = false };
		}

		var mapedResult = _mapper.Map<List<PublicationExternDatabaseId>>(externIds);
		return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = true, ExternDbIdentifiers = mapedResult };
	}
}
