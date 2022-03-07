﻿using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Previews;

public class PublicationPreview
{
	public long Id { get; set; }
	public ICollection<PublicationName>? Names { get; set; }
	public ICollection<PublicationIdentifier>? Identifiers { get; set; }
	public ICollection<PublicationAuthorDetails>? Authors { get; set; }
}
