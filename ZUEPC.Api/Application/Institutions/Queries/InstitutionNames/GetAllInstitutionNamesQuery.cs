using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQuery:
	PaginationWithFilterQueryBase<InstitutionNameFilter>,
	IRequest<GetAllInstitutionNamesQueryResponse>
{
}
