﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationPublicationAuthorsQueryResponse : ResponseBaseWithData<ICollection<PublicationAuthor>>
{
}