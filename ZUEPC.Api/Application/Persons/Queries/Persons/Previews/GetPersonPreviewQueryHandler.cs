using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQueryHandler :
	EPCPersonPreviewQueryHandlerBase,
	IRequestHandler<GetPersonPreviewQuery, GetPersonPreviewQueryResponse>
{

	public GetPersonPreviewQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) { }
	public async Task<GetPersonPreviewQueryResponse> Handle(GetPersonPreviewQuery request, CancellationToken cancellationToken)
	{
		long personId = request.Id;
		Person? personDomain = (await _mediator.Send(new GetPersonQuery() { Id = personId})).Data;
		if(personDomain is null)
		{
			return new() { Success = false };
		}

		PersonPreview resultPreview = await ProcessPersonPreview(personDomain);
		return new() { Success = true, Data = resultPreview };
	}
}
