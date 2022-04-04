using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;

public abstract class EPCInstitutionPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCInstitutionPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<InstitutionPreview> ProcessInstitutionPreview(Institution institutionDomain)
	{
		long institutionId = institutionDomain.Id;
		InstitutionPreview resultPreview = _mapper.Map<InstitutionPreview>(institutionDomain);
		resultPreview.Names = (await _mediator.Send(new GetInstitutionInstitutionNamesQuery()
		{
			InstitutionId = institutionId
		})).Data;
		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetInstitutionInstitutionExternDatabaseIdsQuery()
		{
			InstitutionId = institutionId
		})).Data;
		return resultPreview;
	}
}
