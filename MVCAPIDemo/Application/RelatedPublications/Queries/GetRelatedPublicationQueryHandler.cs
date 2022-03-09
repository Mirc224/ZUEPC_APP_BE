using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQueryHandler : IRequestHandler<GetRelatedPublicationQuery, GetRelatedPublicationQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IRelatedPublicationData _repository;

	public GetRelatedPublicationQueryHandler(IMapper mapper, IRelatedPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetRelatedPublicationQueryResponse> Handle(GetRelatedPublicationQuery request, CancellationToken cancellationToken)
	{
		RelatedPublicationModel? result = await _repository.GetModelByIdAsync(request.RelatedPublicationRecordId);
		if (result is null)
		{
			return new() { Success = false };
		}
		RelatedPublication mappedResult = _mapper.Map<RelatedPublication>(result);
		return new() { Success = true, Data = mappedResult };
	}
}
