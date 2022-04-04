using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQuery :
	PaginationWithFilterQueryBase<PersonFilter>,
	IRequest<GetAllPersonPreviewsQueryResponse>
{
}
