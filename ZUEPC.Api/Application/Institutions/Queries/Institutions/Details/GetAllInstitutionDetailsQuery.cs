using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQuery :
	PaginationWithFilterQueryBase<InstitutionFilter>,
	IRequest<GetAllInstitutionDetailsQueryResponse>
{
}
