using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationExternDbIdsInSetQueryHandler : IRequestHandler<GetAllPublicationExternDbIdsInSetQuery, GetAllPublicationExternDbIdsInSetQueryResponse>
{
	private readonly IPublicationExternDatabaseIdData _repository;

	public GetAllPublicationExternDbIdsInSetQueryHandler(IPublicationExternDatabaseIdData repository)
	{
		_repository = repository;
	}

	public async Task<GetAllPublicationExternDbIdsInSetQueryResponse> Handle(GetAllPublicationExternDbIdsInSetQuery request, CancellationToken cancellationToken)
	{
		var externIds = await _repository.GetAllPublicationExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = false };
		}

		return new GetAllPublicationExternDbIdsInSetQueryResponse() { Success = true, ExternDbIdentifiers = externIds };
	}
}
