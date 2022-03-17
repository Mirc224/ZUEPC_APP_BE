using System.Xml.Linq;

namespace ZUEPC.Application.Import.Commands;

public abstract class ImportXmlBaseCommand
{
	public string? RawContent { get; set; }
	public XElement? XEelementBody { get; set; }
}
