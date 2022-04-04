﻿using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQuery:
	PaginationWithFilterQueryBase<InstitutionNameFilter>,
	IRequest<GetAllInstitutionNamesQueryResponse>
{
}
