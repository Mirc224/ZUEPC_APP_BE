using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQueryHandler :
	EPCInstitutionPreviewQueryHandlerBase,
	IRequestHandler<GetInstitutionPreviewQuery, GetInstitutionPreviewQueryResponse>
{
	public GetInstitutionPreviewQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }
	
	public async Task<GetInstitutionPreviewQueryResponse> Handle(GetInstitutionPreviewQuery request, CancellationToken cancellationToken)
	{
		long institutionId = request.Id;
		Institution? institutionDomain = (await _mediator.Send(new GetInstitutionQuery() { Id = institutionId })).Data;
		if (institutionDomain is null)
		{
			return new() { Success = false };
		}
		InstitutionPreview previewResult = await ProcessInstitutionPreview(institutionDomain);
		return new() { Success = true, Data = previewResult };
	}
}
