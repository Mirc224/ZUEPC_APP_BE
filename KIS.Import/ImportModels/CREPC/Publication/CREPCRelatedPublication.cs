using System.Xml.Serialization;
using ZUEPC.Import.Enums.Publication;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCRelatedPublication
{
	[XmlAttribute(AttributeName = "source")]
	public string? Source { get; set; }
	public PublicationReferenceType ReferenceType { get; set; }
	[XmlAttribute(AttributeName = "bond_type")]
	public string? ReferenceTypeString
	{
		get => _referenceTypeString;
		set
		{
			_referenceTypeString = value;
			Dictionary<string, PublicationReferenceType> referenceTypeMapping = new()
			{
				{ "response_to", PublicationReferenceType.REFERENCE_TO },
				{ "responded_in", PublicationReferenceType.REFERENCE_IN }
			};
			if (value != null && referenceTypeMapping.TryGetValue(value, out var foundValue))
			{
				ReferenceType = foundValue;
				return;
			}
			ReferenceType = default;
		}
	}
	[XmlElement(ElementName = "citation_category")]
	public string? CitationCategory { get; set; }
	[XmlElement(ElementName = "rec_biblio")]
	public CREPCPublication? Publication { get; set; }
	public string? _referenceTypeString { get; set; }
}