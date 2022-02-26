﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDatabaseIdsQueryResponse : ResponseBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<PublicationExternDatabaseId> PublicationExternDatabaseIds { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}