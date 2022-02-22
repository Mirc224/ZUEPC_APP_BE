using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Import.Models;
using ZUEPC.Import.Parser;

namespace ZUEPC.Application.Import.Services;

public class ImportService
{
	public ImportService()
	{

	}


	public void ImportFromCREPCXML(ImportCREPCXmlCommand command)
	{
		var result = ParseImportXMLCommand(command);
		if(result is null || result.Any())
		{
			return;
		}
	}

	public void ImportFromCREPCXML(ImportDaWinciXmlCommand command)
	{
		var result = ParseImportXMLCommand(command);
		if (result is null || result.Any())
		{
			return;
		};
	}

	private IEnumerable<ImportRecord>? ParseImportXMLCommand(ImportXmlBaseCommand command)
	{
		if (command.XMLBody is null)
		{
			return null;
		}

		var doc = XDocument.Parse(command.XMLBody);
		return ImportParser.ParseCREPC(doc);
	}
}
