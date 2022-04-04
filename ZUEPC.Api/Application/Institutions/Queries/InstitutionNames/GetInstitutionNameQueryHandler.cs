using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNameQueryHandler :
	GetSimpleModelQueryHandlerBase<IInstitutionNameData,InstitutionName, InstitutionNameModel, long>,
	IRequestHandler<GetInstitutionNameQuery, GetInstitutionNameQueryResponse>
{
	public GetInstitutionNameQueryHandler(IMapper mapper, IInstitutionNameData repository)
		: base(mapper, repository)	{ }
	

	public async Task<GetInstitutionNameQueryResponse> Handle(GetInstitutionNameQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetInstitutionNameQuery, GetInstitutionNameQueryResponse>(request);
	}
}
