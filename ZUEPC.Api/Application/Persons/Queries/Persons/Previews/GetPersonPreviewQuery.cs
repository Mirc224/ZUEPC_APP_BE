﻿using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPersonPreviewQueryResponse>
{
}
