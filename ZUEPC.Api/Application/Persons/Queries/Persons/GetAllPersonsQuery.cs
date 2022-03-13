﻿using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQuery :
	PaginationQueryWithUriBase,
	IRequest<GetAllPersonsQueryResponse>
{
}