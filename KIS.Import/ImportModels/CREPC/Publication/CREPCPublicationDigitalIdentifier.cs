using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCPublicationDigitalIdentifier : CREPCPublicationIdentifier
{
	[XmlAttribute(AttributeName = "di_type")]
	public override string IdentifierTypeString
	{
		set => base.IdentifierTypeString = value;
		get => base.IdentifierTypeString;
	}
	[XmlElement(ElementName = "digi_value")]
	public override string? IdentifierValue { get; set; }
}