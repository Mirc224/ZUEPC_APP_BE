using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNamesQueryHandler : IRequestHandler<GetPublicationNamesQuery, GetPublicationNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public GetPublicationNamesQueryHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationNamesQueryResponse> Handle(GetPublicationNamesQuery request, CancellationToken cancellationToken)
	{
		var resultModel = await _repository.GetPublicationNamesByPublicationIdAsync(request.PublicationId);

		var mappedResult = _mapper.Map<List<PublicationName>>(resultModel);

		return new GetPublicationNamesQueryResponse() { Success = true, PublicationNames = mappedResult };
	}
}
