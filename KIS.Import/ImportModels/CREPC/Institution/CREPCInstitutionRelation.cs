using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Institution;

public class CREPCInstitutionRelation
{
	[XmlAttribute(AttributeName = "bond_type")]
	public string? RelationType { get; set; }
	[XmlAttribute(AttributeName = "rec_institution")]
	public CREPCInstitution? RelatedInstitution { get; set; }
}