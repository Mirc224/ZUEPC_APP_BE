using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCPublicationInternationalIdentifier : CREPCPublicationIdentifier
{

	[XmlAttribute(AttributeName = "is_type")]
	public override string IdentifierTypeString
	{
		set => base.IdentifierTypeString = value;
		get => base.IdentifierTypeString;
	}
	[XmlElement(ElementName = "number")]
	public override string? IdentifierValue { get; set; }
	[XmlAttribute(AttributeName = "is_form")]
	public string? IdentifierForm { get; set; }
}