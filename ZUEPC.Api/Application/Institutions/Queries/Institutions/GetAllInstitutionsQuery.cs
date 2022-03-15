using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQuery :
	PaginationWithFilterQueryBase<InstitutionFilter>,
	IRequest<GetAllInstitutionsQueryResponse>
{
}
