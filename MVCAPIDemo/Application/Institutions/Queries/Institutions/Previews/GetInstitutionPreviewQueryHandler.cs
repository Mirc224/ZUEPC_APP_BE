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
		long institutionId = request.Id;
		Institution? institutionDomain = (await _mediator.Send(new GetInstitutionQuery() { Id = institutionId })).Data;
		if (institutionDomain is null)
		{
			return new() { Success = false };
		}
		InstitutionPreview previewResult = _mapper.Map<InstitutionPreview>(institutionDomain);
		previewResult.Names = (await _mediator.Send(new GetInstitutionInstitutionNamesQuery() { InstitutionId = institutionId })).Data;
		previewResult.ExternDatabaseIds = (await _mediator.Send(new GetInstitutionInstitutionExternDatabaseIdsQuery() 
		{ InstitutionId = institutionId })).Data;
		return new() { Success = true, Data = previewResult };
	}
}
