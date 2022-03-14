using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQueryHandler :
	GetSimpleModelQueryHandlerBase<Institution, InstitutionModel>,
	IRequestHandler<GetInstitutionQuery, GetInstitutionQueryResponse>
{
	public GetInstitutionQueryHandler(IMapper mapper, IInstitutionData repository)
		:base(mapper, repository) { }
	
	public async Task<GetInstitutionQueryResponse> Handle(GetInstitutionQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetInstitutionQuery, GetInstitutionQueryResponse>(request);
	}
}
