﻿using ZUEPC.EvidencePublication.Domain.Institutions;
using ZUEPC.Responses;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQueryResponse :
	PagedResponseBase<IEnumerable<Institution>>
{
}