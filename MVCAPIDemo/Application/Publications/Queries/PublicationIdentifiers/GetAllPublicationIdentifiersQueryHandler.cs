using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersQueryHandler : IRequestHandler<GetAllPublicationIdentifiersQuery, GetAllPublicationIdentifiersQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public GetAllPublicationIdentifiersQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetAllPublicationIdentifiersQueryResponse> Handle(GetAllPublicationIdentifiersQuery request, CancellationToken cancellationToken)
	{
		var publicationIdentifierModels = _repository.GetAllPublicationIdentifiersByPublicationIdAsync(request.PublicationId);
		var mappedResult = _mapper.Map<List<PublicationIdentifier>>(publicationIdentifierModels);

		return new() { Success = true, PublicationIdentifiers = mappedResult};

	}
}
