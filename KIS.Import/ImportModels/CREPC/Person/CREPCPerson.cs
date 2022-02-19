using System.Xml.Serialization;
using ZUEPC.Import.ImportModels.CREPC.Common;

namespace ZUEPC.Import.ImportModels.CREPC.Person;

[XmlRoot(ElementName = "rec_person", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
public class CREPCPerson
{
	[XmlAttribute(AttributeName = "id")]
	public int CREPCId { get; set; }
	
	[XmlElement(ElementName = "firstname")]
	public string? FirstName { get; set; }
	
	[XmlElement(ElementName = "lastname")]
	public string? LastName { get; set; }
	
	[XmlElement(ElementName = "middle_name")]
	public string? MiddleName { get; set; }
	
	[XmlElement(ElementName = "cross_person_database")]
	public CREPCPersonIdInOtherDatabase[]? PersonsIdInOtherDatabases { get; set; }
	
	[XmlElement(ElementName = "cross_person_institution")]
	public CREPCPersonInstitution[]? PersonsInstitution { get; set; }

	[XmlArray(ElementName = "periods")]
	[XmlArrayItem(ElementName = "period")]
	public CREPCDatePeriodInterval[]? PesronsBirthAndDeathDates { get; set; }
}