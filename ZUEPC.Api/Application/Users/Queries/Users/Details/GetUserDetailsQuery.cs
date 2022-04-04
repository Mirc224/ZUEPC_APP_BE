﻿using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetUserDetailsQuery :
	EPCSimpleQueryBase<long>,
	IRequest<GetUserDetailsQueryResponse>
{
}
