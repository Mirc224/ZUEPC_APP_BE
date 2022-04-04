using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNameQuery :
	EPCQueryWithIdBase<long>,
	IRequest<GetInstitutionNameQueryResponse>
{
}
