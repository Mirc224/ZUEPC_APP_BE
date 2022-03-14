using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQuery :
	PaginationWithUriQueryBase,
	IRequest<GetAllInstitutionsQueryResponse>
{
}
