﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommandResponse : ResponseBase
{
	public PublicationExternDatabaseId CreatedPublicationExternDatabaseId { get; set; }
}