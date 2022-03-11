using System.Xml.Linq;

namespace ZUEPC.Application.Import.Commands;

public abstract class ImportXmlBaseCommand
{
	public XElement? XEelementBody { get; set; }
}
