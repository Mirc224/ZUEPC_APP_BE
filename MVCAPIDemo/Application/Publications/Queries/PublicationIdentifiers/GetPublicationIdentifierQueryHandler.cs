using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQueryHandler : IRequestHandler<GetPublicationIdentifierQuery, GetPublicationIdentifierQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public GetPublicationIdentifierQueryHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationIdentifierQueryResponse> Handle(GetPublicationIdentifierQuery request, CancellationToken cancellationToken)
	{
		PublicationIdentifierModel? result = await _repository.GetPublicationIdentifierByIdAsync(request.PublicationIdentifierRecordId);
		if (result is null)
		{
			return new() { Success = false };
		}
		PublicationIdentifier mappedResult = _mapper.Map<PublicationIdentifier>(result);
		return new() { Success = true, PublicationIdentifier = mappedResult };
	}
}
