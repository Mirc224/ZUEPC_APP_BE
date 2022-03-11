using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationPublicationNamesQueryHandler : IRequestHandler<GetPublicationPublicationNamesQuery, GetPublicationPublicationNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public GetPublicationPublicationNamesQueryHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationPublicationNamesQueryResponse> Handle(GetPublicationPublicationNamesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationNameModel> resultModel = await _repository.GetPublicationNamesByPublicationIdAsync(request.PublicationId);

		List<PublicationName> mappedResult = _mapper.Map<List<PublicationName>>(resultModel);

		return new GetPublicationPublicationNamesQueryResponse() { Success = true, Data = mappedResult };
	}
}
