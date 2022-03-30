using AutoMapper;
using MediatR;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews.BaseHandlers;

public abstract class EPCPublicationPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPublicationPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PublicationPreview> ProcessPublicationPreview(Publication publicationDomain)
	{
		long publicationId = publicationDomain.Id;
		PublicationPreview resultPreview = _mapper.Map<PublicationPreview>(publicationDomain);
		resultPreview.Names = (await _mediator.Send(new GetPublicationPublicationNamesQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.Identifiers = (await _mediator.Send(new GetPublicationPublicationIdentifiersQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.Authors = (await _mediator.Send(new GetPublicationAuthorDetailsQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetPublicationPublicationExternDatabaseIdsQuery()
		{
			PublicationId = publicationId
		})).Data;

		return resultPreview;
	}
}
