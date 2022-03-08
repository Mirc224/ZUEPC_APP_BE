﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQueryResponse : ResponseBase
{
	public PublicationIdentifier? PublicationIdentifier { get; set; }
}