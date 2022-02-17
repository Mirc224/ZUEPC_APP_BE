using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Person;

public class CREPCPersonIdInOtherDatabase
{
	[XmlAttribute(AttributeName = "id_value")]
	public string? Id { get; set; }
	[XmlAttribute(AttributeName = "id_title")]
	public string? IdDescription { get; set; }
}
