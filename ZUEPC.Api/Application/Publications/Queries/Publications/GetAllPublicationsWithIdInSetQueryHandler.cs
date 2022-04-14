using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.Publications;

public class GetAllPublicationsWithIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationsWithIdInSetQuery, GetAllPublicationsWithIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationData _repository;

	public GetAllPublicationsWithIdInSetQueryHandler(IMapper mapper, IPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPublicationsWithIdInSetQueryResponse> Handle(GetAllPublicationsWithIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationModel> publications = await _repository.GetModelsWhereIdInSetAsync(request.PublicationIds);
		if (publications is null)
		{
			return new() { Success = false };
		}

		IEnumerable<Publication> mappedResult = _mapper.Map<List<Publication>>(publications);
		return new() { Success = true, Data = mappedResult };
	}
}
