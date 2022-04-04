using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetInstitutionQueryResponse>
{
}
