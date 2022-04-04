using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details.BaseHandlers;

public abstract class EPCInstitutionDetailsQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCInstitutionDetailsQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<InstitutionDetails> ProcessInstitutionDetails(Institution institutionDomain)
	{
		long institutionId = institutionDomain.Id;
		InstitutionDetails result = _mapper.Map<InstitutionDetails>(institutionDomain);
		result.Names = (await _mediator.Send(new GetInstitutionInstitutionNamesQuery()
		{
			InstitutionId = institutionId
		})).Data;

		result.ExternDatabaseIds = (await _mediator.Send(new GetInstitutionInstitutionExternDatabaseIdsQuery()
		{
			InstitutionId = institutionId
		})).Data;
		return result;
	}
}
