using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Common;

public class CREPCDatePeriod
{
	public string? PeriodType => Year.PeriodType ?? Month.PeriodType ?? Day.PeriodType ?? default;
	[XmlElement("year")]
	public CREPCDate Year { get; set; } = new() { Value = 1970 };
	[XmlElement("month")]
	public CREPCDate Month { get; set; } = new() { Value = 1 };
	[XmlElement("day")]
	public CREPCDate Day { get; set; } = new() { Value = 1 };
	public DateTime? Date => new DateTime(Year.Value, Month.Value, Day.Value);
}