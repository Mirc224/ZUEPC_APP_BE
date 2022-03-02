using AutoMapper;
using MediatR;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQueryHandler : IRequestHandler<GetPublicationPreviewQuery, GetPublicationPreviewQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationPreviewQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<GetPublicationPreviewQueryResponse> Handle(GetPublicationPreviewQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;
		Publication? publicationDomain = (await _mediator.Send(new GetPublicationQuery() { PublicationId = publicationId })).Publication;
		if (publicationDomain is null)
		{
			return new() { Success = false };
		}
		PublicationPreview resultPreview = _mapper.Map<PublicationPreview>(publicationDomain);
		resultPreview.PublicationNames = (await _mediator.Send(new GetPublicationNamesQuery()
		{
			PublicationId = publicationId
		})).PublicationNames;

		resultPreview.PublicationIdentifiers = (await _mediator.Send(new GetPublicationIdentifiersQuery()
		{
			PublicationId = publicationId
		})).PublicationIdentifiers;

		resultPreview.PublicationAuthors = (await _mediator.Send(new GetPublicationAuthorDetailsQuery()
		{
			PublicationId = publicationId
		})).PublicationAuthorDetails;

		return new() { Success = true, PublicationPreview = resultPreview };
	}
}
