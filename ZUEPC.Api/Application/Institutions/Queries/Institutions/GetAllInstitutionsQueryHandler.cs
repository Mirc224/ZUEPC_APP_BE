using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQueryHandler :
	GetModelsPagedQueryHandlerBase<IInstitutionData, Institution, InstitutionModel>,
	IRequestHandler<GetAllInstitutionsQuery, GetAllInstitutionsQueryResponse>
{
	public GetAllInstitutionsQueryHandler(IMapper mapper, IInstitutionData repository)
	: base(mapper, repository) { }

	public async Task<GetAllInstitutionsQueryResponse> Handle(GetAllInstitutionsQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetAllInstitutionsQuery, GetAllInstitutionsQueryResponse>(request);
	}
}
