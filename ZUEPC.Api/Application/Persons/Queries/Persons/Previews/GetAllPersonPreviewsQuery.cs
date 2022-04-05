using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQuery :
	PaginatedQueryWithFilterBase<PersonFilter>,
	IRequest<GetAllPersonPreviewsQueryResponse>
{
}
