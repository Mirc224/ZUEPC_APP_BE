using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQuery : 
	QueryWithIdBase<long>,
	IRequest<GetInstitutionQueryResponse>
{
}
