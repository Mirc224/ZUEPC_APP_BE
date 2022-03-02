using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifiersQueryHandler : IRequestHandler<GetPublicationIdentifiersQuery, GetPublicationIdentifiersQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public GetPublicationIdentifiersQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationIdentifiersQueryResponse> Handle(GetPublicationIdentifiersQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationIdentifierModel> publicationIdentifierModels = await _repository.GetPublicationIdentifiersByPublicationIdAsync(request.PublicationId);
		ICollection<PublicationIdentifier> mappedResult = _mapper.Map<List<PublicationIdentifier>>(publicationIdentifierModels);

		return new() { Success = true, PublicationIdentifiers = mappedResult };

	}
}
