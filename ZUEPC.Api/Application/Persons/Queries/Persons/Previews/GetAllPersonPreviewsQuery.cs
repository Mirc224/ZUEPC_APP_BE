using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQuery :
	PaginationWithFilterQueryBase<PersonFilter>,
	IRequest<GetAllPersonPreviewsQueryResponse>
{
}
