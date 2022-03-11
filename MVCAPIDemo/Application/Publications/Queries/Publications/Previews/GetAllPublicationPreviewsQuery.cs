﻿using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQuery : EPCPaginationQueryWithUriBase, IRequest<GetAllPublicationPreviewsQueryResponse>
{
}
