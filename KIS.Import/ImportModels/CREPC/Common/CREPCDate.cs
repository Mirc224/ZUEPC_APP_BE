using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Common;
public class CREPCDate
{
	[XmlText]
	public int Value { get; set; } = 1;
	[XmlAttribute(AttributeName = "period_type")]
	public string? PeriodType { get; set; }
}