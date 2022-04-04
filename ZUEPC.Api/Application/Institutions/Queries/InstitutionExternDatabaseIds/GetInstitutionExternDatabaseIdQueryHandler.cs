using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdQueryHandler:
	GetSimpleModelQueryHandlerBase<IInstitutionExternDatabaseIdData, InstitutionExternDatabaseId, InstitutionExternDatabaseIdModel, long>,
	IRequestHandler<GetInstitutionExternDatabaseIdQuery, GetInstitutionExternDatabaseIdQueryResponse>
{
	public GetInstitutionExternDatabaseIdQueryHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
		: base(mapper, repository) { }
	
	public async Task<GetInstitutionExternDatabaseIdQueryResponse> Handle(GetInstitutionExternDatabaseIdQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetInstitutionExternDatabaseIdQuery, GetInstitutionExternDatabaseIdQueryResponse>(request);
	}
}
