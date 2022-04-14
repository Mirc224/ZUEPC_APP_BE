using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationIdentifiersByPublicationIdInSetQuery, GetAllPublicationIdentifiersByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public GetAllPublicationIdentifiersByPublicationIdInSetQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetAllPublicationIdentifiersByPublicationIdInSetQueryResponse> Handle(GetAllPublicationIdentifiersByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationIdentifierModel> identifiers = await _repository.GetAllPublicationIdentifierByPublicationIdInSetAsync(request.PublicationIds);
		if (identifiers is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PublicationIdentifier> mappedResult = _mapper.Map<List<PublicationIdentifier>>(identifiers);
		return new() { Success = true, Data = mappedResult };
	}
}
