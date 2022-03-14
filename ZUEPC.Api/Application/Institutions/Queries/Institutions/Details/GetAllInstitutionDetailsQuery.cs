using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQuery :
	PaginationWithUriQueryBase, 
	IRequest<GetAllInstitutionDetailsQueryResponse>
{
}
