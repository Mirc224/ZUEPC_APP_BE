using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Institution;

public class CREPCInstitutionName
{
	[XmlAttribute(AttributeName = "inst_type")]
	public string? NameType { get; set; }
	[XmlText]
	public string? Name { get; set; }
	[XmlAttribute(AttributeName = "lang")]
	public string? Language { get; set; }
}