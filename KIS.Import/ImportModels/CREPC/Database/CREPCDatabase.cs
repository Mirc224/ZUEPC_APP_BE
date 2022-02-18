using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Database;

[XmlRoot(ElementName = "rec_database", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
public class CREPCDatabase
{
	[XmlAttribute(AttributeName = "id")]
	public string? CREPCId { get; set; }
	[XmlElement(ElementName = "name")]
	public CREPCDatabaseName[]? DatabaseNames { get; set; }
}
