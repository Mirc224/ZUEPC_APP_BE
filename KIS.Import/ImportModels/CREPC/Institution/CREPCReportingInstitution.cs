using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Institution;

public class CREPCReportingInstitution
{
	[XmlAttribute(AttributeName = "cross_id")]
	public string? CREPCPersonReportingInstitutionRecordId { get; set; }
	[XmlAttribute(AttributeName = "bond_type")]
	public string? BindingType { get; set; }
	[XmlElement(ElementName = "rec_institution")]
	public CREPCInstitution? ReportingInstitution { get; set; }
}