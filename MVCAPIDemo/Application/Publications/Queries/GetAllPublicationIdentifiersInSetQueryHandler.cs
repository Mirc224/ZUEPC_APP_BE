using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationIdentifiersInSetQueryHandler : IRequestHandler<GetAllPublicationIdentifiersInSetQuery, GetAllPublicationIdentifiersInSetQueryResponse>
{
	private readonly IPublicationIdentifierData _repository;

	public GetAllPublicationIdentifiersInSetQueryHandler(IPublicationIdentifierData repository)
	{
		_repository = repository;
	}
	public async Task<GetAllPublicationIdentifiersInSetQueryResponse> Handle(GetAllPublicationIdentifiersInSetQuery request, CancellationToken cancellationToken)
	{
		var identifiers = await _repository.GetAllPublicationIdentifiersByIdentifierValueSetAsync(request.SearchedIdentifiers);

		if (identifiers is null)
		{
			return new() { Success = false };
		}

		var result = new GetAllPublicationIdentifiersInSetQueryResponse()
		{
			Success = true,
			Identifiers = identifiers
		};
		return result;
	}
}
