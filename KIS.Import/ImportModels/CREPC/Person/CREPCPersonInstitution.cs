using System.Xml.Serialization;
using ZUEPC.Import.Enums.Person;
using ZUEPC.Import.ImportModels.CREPC.Common;

namespace ZUEPC.Import.ImportModels.CREPC.Person;

public class CREPCPersonInstitution
{
	[XmlAttribute(AttributeName = "cross_id")]
	public string? CREPCPersonInstitutionId { get; set; }
	public PersonToInstitutionRelationType? PersonToInstitutionRelationType { get; set; }
	[XmlElement(ElementName = "position")]
	public string? Position { get; set; }
	[XmlElement(ElementName = "work_load")]
	public string? WorkLoadString
	{
		get => WorkLoad.ToString();
		set
		{
			WorkLoad = default;
			string currentDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			var convertedVal = value?.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
			if (double.TryParse(convertedVal, out var tmpNumber))
			{
				WorkLoad = tmpNumber;
			}
		}
	}
	[XmlAttribute(AttributeName = "cc_type")]
	public string? ConnectionTypeString
	{
		get => _connectionTypeString;
		set
		{
			_connectionTypeString = value;
		}
	}


	[XmlArray(ElementName = "periods")]
	[XmlArrayItem(ElementName = "date")]
	public CREPCDatePeriod[]? OccupationDates { get; set; }

	public double? WorkLoad { get; set; }
	public string? _connectionTypeString;
}