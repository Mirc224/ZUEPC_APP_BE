using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQueryHandler : IRequestHandler<GetAllPublicationNamesQuery, GetAllPublicationNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public GetAllPublicationNamesQueryHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetAllPublicationNamesQueryResponse> Handle(GetAllPublicationNamesQuery request, CancellationToken cancellationToken)
	{
		var resultModel = await _repository.GetPublicationNamesByPublicationIdAsync(request.PublicationId);

		var mappedResult = _mapper.Map<List<PublicationName>>(resultModel);

		return new GetAllPublicationNamesQueryResponse() { Success = true, PublicationNames = mappedResult };
	}
}
