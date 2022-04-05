using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQuery :
	QueryWithIdBase<long>,
	IRequest<GetInstitutionDetailsQueryResponse>
{
}
