using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQueryHandler : IRequestHandler<GetPublicationExternDatabaseIdQuery, GetPublicationExternDatabaseIdQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public GetPublicationExternDatabaseIdQueryHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	
	public async Task<GetPublicationExternDatabaseIdQueryResponse> Handle(GetPublicationExternDatabaseIdQuery request, CancellationToken cancellationToken)
	{
		PublicationExternDatabaseIdModel? result = await _repository.GetModelByIdAsync(request.PublicationExternDatabaseIdId);
		if (result is null)
		{
			return new() { Success = false };
		}
		PublicationExternDatabaseId mappedResult = _mapper.Map<PublicationExternDatabaseId>(result);
		return new() { Success = true, Data = mappedResult };
	}
}
