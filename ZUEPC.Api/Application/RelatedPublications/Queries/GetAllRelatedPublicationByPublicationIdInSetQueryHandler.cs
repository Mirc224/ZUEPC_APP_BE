using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Api.Application.RelatedPublications.Queries;

public class GetAllRelatedPublicationByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllRelatedPublicationByPublicationIdInSetQuery, GetAllRelatedPublicationByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IRelatedPublicationData _repository;

	public GetAllRelatedPublicationByPublicationIdInSetQueryHandler(IMapper mapper, IRelatedPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllRelatedPublicationByPublicationIdInSetQueryResponse> Handle(GetAllRelatedPublicationByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<RelatedPublicationModel> relatedPublications = await _repository.GetAllRelatedPublicationsByPublicationIdInSetAsync(request.PublicationIds);
		if (relatedPublications is null)
		{
			return new() { Success = false };
		}

		IEnumerable<RelatedPublication> mappedResult = _mapper.Map<List<RelatedPublication>>(relatedPublications);
		return new() { Success = true, Data = mappedResult };
	}
}
