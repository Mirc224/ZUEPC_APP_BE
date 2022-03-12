using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQuery :
	PaginationQueryWithUriBase,
	IRequest<GetAllInstitutionsQueryResponse>
{
}
