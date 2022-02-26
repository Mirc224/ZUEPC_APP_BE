﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetPersonQueryResponse : ResponseBase
{
	public Person? Person { get; set; }
}