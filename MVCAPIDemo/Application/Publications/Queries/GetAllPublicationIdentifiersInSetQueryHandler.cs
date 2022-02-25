using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries;

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
		var identifiers = await _repository.GetAllPublicationIdentifiersByIdentifierValueSetAsync(request.SearchedIdentifiers);

		if (identifiers is null)
		{
			return new() { Success = false };
		}

		var mappedResult = _mapper.Map<List<PublicationIdentifier>>(identifiers);
		var result = new GetAllPublicationIdentifiersInSetQueryResponse()
		{
			Success = true,
			Identifiers = mappedResult
		};
		return result;
	}
}
