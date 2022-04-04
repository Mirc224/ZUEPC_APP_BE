using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQuery :
	PaginationWithFilterQueryBase<InstitutionFilter>,
	IRequest<GetAllInstitutionDetailsQueryResponse>
{
}
