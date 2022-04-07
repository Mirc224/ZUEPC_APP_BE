using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries.Details;

public class GetPublicationRelatedPublicationsDetailsQueryHandler : IRequestHandler<GetPublicationRelatedPublicationsDetailsQuery, GetPublicationRelatedPublicationsDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationRelatedPublicationsDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetPublicationRelatedPublicationsDetailsQueryResponse> Handle(GetPublicationRelatedPublicationsDetailsQuery request, CancellationToken cancellationToken)
	{
		long sourcePublicationId = request.SourcePublicationId;
		IEnumerable<RelatedPublication>? relatedPublicationsDomain = (await _mediator.Send(new GetPublicationRelatedPublicationsQuery() 
		{ 
			SourcePublicationId = sourcePublicationId 
		})).Data;
		if (relatedPublicationsDomain is null)
		{
			return new() { Success = false};
		}

		List<RelatedPublicationDetails>? resultRelatedPub = new();
		foreach(RelatedPublication relatedPublication in relatedPublicationsDomain)
		{
			long destinationId = relatedPublication.RelatedPublicationId;
			RelatedPublicationDetails mappedResult = _mapper.Map<RelatedPublicationDetails>(relatedPublication);
			mappedResult.RelatedPublication = (await _mediator.Send(new GetPublicationPreviewQuery() 
			{ 
				Id = destinationId 
			})).Data;
			resultRelatedPub.Add(mappedResult);
		}
		return new() { Success = true, Data = resultRelatedPub };
	}
}
