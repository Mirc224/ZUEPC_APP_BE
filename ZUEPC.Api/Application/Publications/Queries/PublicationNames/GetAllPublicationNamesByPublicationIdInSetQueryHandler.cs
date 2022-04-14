using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationNamesByPublicationIdInSetQuery, GetAllPublicationNamesByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public GetAllPublicationNamesByPublicationIdInSetQueryHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPublicationNamesByPublicationIdInSetQueryResponse> Handle(GetAllPublicationNamesByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationNameModel> names = await _repository.GetAllPublicationNamesByPublicationIdInSetAsync(request.PublicationIds);
		if (names is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PublicationName> mappedResult = _mapper.Map<List<PublicationName>>(names);
		return new() { Success = true, Data = mappedResult };
	}
}
