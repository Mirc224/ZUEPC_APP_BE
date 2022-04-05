using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNameQuery :
	QueryWithIdBase<long>,
	IRequest<GetInstitutionNameQueryResponse>
{
}
