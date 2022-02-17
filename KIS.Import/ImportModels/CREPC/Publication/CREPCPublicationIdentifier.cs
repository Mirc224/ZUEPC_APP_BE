using System.Xml.Serialization;
using ZUEPC.Import.Enums.Publication;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public abstract class CREPCPublicationIdentifier
{
	public virtual PublicationIdentifierType IdentifierType { get; set; }
	[XmlIgnore]
	public virtual string? IdentifierValue { get; set; }
	[XmlIgnore]
	public virtual string IdentifierTypeString
	{
		get => IdentifierType.ToString();
		set
		{
			Dictionary<string, PublicationIdentifierType> identifierType = new()
			{
				{ "isbn", PublicationIdentifierType.ISBN },
				{ "ismn", PublicationIdentifierType.ISMN},
				{ "isrc", PublicationIdentifierType.ISRC},
				{ "isrn", PublicationIdentifierType.ISRN},
				{ "issn", PublicationIdentifierType.ISSN},
				{ "DOI", PublicationIdentifierType.DOI },
			};
			if (identifierType.TryGetValue(value, out var publicationType))
			{
				IdentifierType = publicationType;
				return;
			}
			IdentifierType = default;
		}
	}

}