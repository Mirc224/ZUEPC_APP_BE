using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQuery :
	EPCPaginationQueryWithUriBase, 
	IRequest<GetAllInstitutionDetailsQueryResponse>
{
}
