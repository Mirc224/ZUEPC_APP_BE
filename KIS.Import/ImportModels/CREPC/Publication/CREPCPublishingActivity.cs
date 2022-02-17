using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCPublishingActivity
{
	[XmlAttribute(AttributeName = "id")]
	public string? CREPCId { get; set; }
	[XmlElement(ElementName = "category")]
	public string? Category { get; set; }
	[XmlElement(ElementName = "government_grant")]
	public bool? GovernmentGrant { get; set; }

}
