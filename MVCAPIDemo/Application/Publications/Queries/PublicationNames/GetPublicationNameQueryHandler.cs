using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQueryHandler : IRequestHandler<GetPublicationNameQuery, GetPublicationNameQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public GetPublicationNameQueryHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	
	public async Task<GetPublicationNameQueryResponse> Handle(GetPublicationNameQuery request, CancellationToken cancellationToken)
	{
		PublicationNameModel? result = await _repository.GetPublicationNameByIdAsync(request.PublicatioNameRecordId);
		if (result is null)
		{
			return new() { Success = false };
		}
		PublicationName mappedResult = _mapper.Map<PublicationName>(result);
		return new() { Success = true, PublicationName = mappedResult };
	}
}
