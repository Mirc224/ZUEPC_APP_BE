using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetPublicationRelatedPublicationsCommandHandler : IRequestHandler<GetPublicationRelatedPublicationsCommand, GetPublicationRelatedPublicationsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IRelatedPublicationData _repository;

	public GetPublicationRelatedPublicationsCommandHandler(IMapper mapper, IRelatedPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationRelatedPublicationsCommandResponse> Handle(GetPublicationRelatedPublicationsCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<RelatedPublicationModel> queryResult = await _repository.GetRelatedPublicationsByPublicationIdAsync(request.SourcePublicationId);
		List<RelatedPublication> mappedResult = _mapper.Map<List<RelatedPublication>>(queryResult);

		return new() { Success = true, RelatedPublications = mappedResult };
	}
}
