using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Common;

public class CREPCDatePeriodInterval
{
	[XmlElement(ElementName = "date")]
	public CREPCDatePeriod[]? PeriodDates { get; set; }
}