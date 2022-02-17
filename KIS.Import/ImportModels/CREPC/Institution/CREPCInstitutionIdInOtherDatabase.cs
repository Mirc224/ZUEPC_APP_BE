using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Institution;

public class CREPCInstitutionIdInOtherDatabase
{
	[XmlElement(ElementName = "number")]
	public string? Id { get; set; }
	[XmlElement(ElementName = "num_title")]
	public string? IdDescription { get; set; }
}
