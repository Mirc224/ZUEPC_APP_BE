﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQueryResponse : ResponseBase
{
	public Institution? Institution { get; set; }
}