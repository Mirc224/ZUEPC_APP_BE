﻿using ZUEPC.Common.Responses;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationExternDbIdsInSetQueryResponse : ResponseBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<PublicationExternDatabaseIdModel>? ExternDbIdentifiers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}