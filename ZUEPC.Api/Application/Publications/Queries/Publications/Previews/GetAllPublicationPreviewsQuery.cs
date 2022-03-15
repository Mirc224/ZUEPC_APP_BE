﻿using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQuery :
	PaginationWithFilterQueryBase<PublicationFilter>, 
	IRequest<GetAllPublicationPreviewsQueryResponse>
{
}
