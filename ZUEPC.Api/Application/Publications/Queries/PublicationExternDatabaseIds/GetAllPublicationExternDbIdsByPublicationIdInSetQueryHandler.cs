using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDbIdsByPublicationIdInSetQueryHandler:
	IRequestHandler<GetAllPublicationExternDbIdsByPublicationIdInSetQuery, GetAllPublicationExternDbIdsByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public GetAllPublicationExternDbIdsByPublicationIdInSetQueryHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPublicationExternDbIdsByPublicationIdInSetQueryResponse> Handle(GetAllPublicationExternDbIdsByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationExternDatabaseIdModel> externDbIds = await _repository.GetAllPublicationExternDbIdsByPublicationIdInSetAsync(request.PublicationIds);
		if (externDbIds is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PublicationExternDatabaseId> mappedResult = _mapper.Map<List<PublicationExternDatabaseId>>(externDbIds);
		return new() { Success = true, Data = mappedResult };
	}
}
