﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdsQueryResponse : ResponseBaseWithData<ICollection<PersonExternDatabaseId>>
{
}