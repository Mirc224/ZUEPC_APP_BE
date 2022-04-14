using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Base.Extensions;
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

	protected async Task<IEnumerable<InstitutionDetails>> ProcessInstitutionDetails(IEnumerable<Institution> institutionDomains)
	{
		IEnumerable<InstitutionDetails> result = _mapper.Map<List<InstitutionDetails>>(institutionDomains);
		IEnumerable<long> personIds = result.Select(x => x.Id).ToHashSet();

		IEnumerable<InstitutionName> allNamesByInstitutionIds = await GetInstitutionNamesWithInstitutionIdInSet(personIds);
		IEnumerable<InstitutionExternDatabaseId> allExternDbIdsByInstitutionIds = await GetInstitutionExternDbIdsWithInstitutionIdInSet(personIds);

		IEnumerable<IGrouping<long, InstitutionName>> namesGroupByInstitutionId = allNamesByInstitutionIds.GroupBy(x => x.InstitutionId);
		IEnumerable<IGrouping<long, InstitutionExternDatabaseId>> externDbIdsGroupByInstitutionId = allExternDbIdsByInstitutionIds.GroupBy(x => x.InstitutionId);

		foreach (InstitutionDetails institution in result)
		{
			institution.Names = namesGroupByInstitutionId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			institution.ExternDatabaseIds = externDbIdsGroupByInstitutionId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}
		return result;
	}

	private async Task<IEnumerable<InstitutionName>> GetInstitutionNamesWithInstitutionIdInSet(IEnumerable<long> institutionIds)
	{
		return (await _mediator.Send(new GetAllInstititutionNamesByInstititutionIdInSetQuery() { InstitutionIds = institutionIds }))
		.Data
		.OrEmptyIfNull();
	}

	private async Task<IEnumerable<InstitutionExternDatabaseId>> GetInstitutionExternDbIdsWithInstitutionIdInSet(IEnumerable<long> institutionIds)
	{
		return (await _mediator.Send(new GetAllInstititutionExternDbIdsByInstititutionIdInSetQuery() { InstitutionIds = institutionIds }))
		.Data
		.OrEmptyIfNull();
	}
}
