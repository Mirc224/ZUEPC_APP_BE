using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQueryHandler : IRequestHandler<GetInstitutionPreviewQuery, GetInstitutionPreviewQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetInstitutionPreviewQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetInstitutionPreviewQueryResponse> Handle(GetInstitutionPreviewQuery request, CancellationToken cancellationToken)
	{
		long institutionId = request.InstitutionId;
		Institution? institutionDomain = (await _mediator.Send(new GetInstitutionQuery() { InstitutionId = institutionId })).Institution;
		if (institutionDomain is null)
		{
			return new() { Success = false };
		}
		InstitutionPreview previewResult = _mapper.Map<InstitutionPreview>(institutionDomain);
		previewResult.Names = (await _mediator.Send(new GetInstitutionNamesQuery() { InstitutionId = institutionId })).InstitutionNames;
		previewResult.ExternDatabaseIds = (await _mediator.Send(new GetInstitutionExternDatabaseIdsQuery() 
		{ InstitutionId = institutionId })).InstitutionExternDatabaseIds;
		return new() { Success = true, InstitutionPreview = previewResult };
	}
}
