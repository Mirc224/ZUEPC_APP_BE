using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Database;

public class CREPCDatabaseName
{
	[XmlAttribute("name_type")]
	public string? NameType { get; set; }
	[XmlText]
	public string? Name { get; set; }
}
