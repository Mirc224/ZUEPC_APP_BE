using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.Publications.Previews.BaseHandlers;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQueryHandler :
	EPCPublicationPreviewQueryHandlerBase,
	IRequestHandler<GetPublicationPreviewQuery, GetPublicationPreviewQueryResponse>
{
	public GetPublicationPreviewQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetPublicationPreviewQueryResponse> Handle(GetPublicationPreviewQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.Id;
		Publication? publicationDomain = (await _mediator.Send(new GetPublicationQuery() { Id = publicationId })).Data;
		if (publicationDomain is null)
		{
			return new() { Success = false };
		}
		PublicationPreview resultPreview = await ProcessPublicationPreview(publicationDomain);

		return new() { Success = true, Data = resultPreview };
	}
}
