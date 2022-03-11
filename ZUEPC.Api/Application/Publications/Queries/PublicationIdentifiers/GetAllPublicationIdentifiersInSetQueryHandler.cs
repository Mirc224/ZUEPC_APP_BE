using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersInSetQueryHandler : IRequestHandler<GetAllPublicationIdentifiersInSetQuery, GetAllPublicationIdentifiersInSetQueryResponse>
{
	private readonly IPublicationIdentifierData _repository;
	private readonly IMapper _mapper;

	public GetAllPublicationIdentifiersInSetQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<GetAllPublicationIdentifiersInSetQueryResponse> Handle(GetAllPublicationIdentifiersInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationIdentifierModel>? identifiers = await _repository.GetAllPublicationIdentifiersByIdentifierValueSetAsync(request.SearchedIdentifiers);

		if (identifiers is null)
		{
			return new() { Success = false };
		}

		List<PublicationIdentifier> mappedResult = _mapper.Map<List<PublicationIdentifier>>(identifiers);
		GetAllPublicationIdentifiersInSetQueryResponse result = new GetAllPublicationIdentifiersInSetQueryResponse()
		{
			Success = true,
			Data = mappedResult
		};
		return result;
	}
}
