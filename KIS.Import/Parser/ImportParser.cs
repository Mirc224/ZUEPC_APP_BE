using System.Xml.Linq;
using ZUEPC.Import.Models;

namespace ZUEPC.Import.Parser;

public partial class ImportParser
{
	private const string DAWINCI_CONTROLFIELD = "controlfield";
	private const string DAWINCI_DATAFIELD = "datafield";
	private const string DAWINCI_SUBFIELD = "subfield";
	private const string DAWINCI_CODE = "code";
	private const string DAWINCI_TAG = "tag";
	private const string ZU_PERSONID_PREFIX = "ŽU Žilina";
	private const string ZU_PUBLICATIONID_PREFIX = "ŽU.Žilina.";

	private const string CREPC_IDENTIFIER_PREFIX = "crepc2";
	
	public static IEnumerable<ImportRecord> ParseCREPC(XDocument doc)
	{
		string biblibsearch = "http://biblib.net/search/";
		string xmlns = "http://www.crepc.sk/schema/xml-crepc2/";

		List<ImportRecord> result = new();
		var allRecords = doc.Descendants(XName.Get("record", biblibsearch));

		foreach (var node in allRecords)
		{
			var parsedRecord = ParseCREPCImportRecord(node, biblibsearch, xmlns);
			if (parsedRecord is null)
			{
				continue;
			}
			result.Add(parsedRecord);
		}
		return result;
	}


	private static ImportRecord? ParseCREPCImportRecord(XElement record, string biblibsearch, string xmlns)
	{
		var importedRecord = new ImportRecord();
		importedRecord.RecordVersionDateString = record.Descendants(XName.Get("updated", biblibsearch)).FirstOrDefault()?.Value;
		var publicationElement = record.Descendants(XName.Get("rec_biblio", xmlns)).FirstOrDefault();
		if (publicationElement is null)
		{
			return default;
		}
		importedRecord.Publication = ParseCREPCPublication(publicationElement, xmlns);

		return importedRecord;
	}


	private static int? ParseInt(string? value)
	{
		if (int.TryParse(value, out var result))
		{
			return result;
		}
		return null;
	}

	private static DateTime? ParseCREPCDate(XElement dateElement, string xmlns)
	{
		if (!int.TryParse(dateElement.Element(XName.Get("year", xmlns))?.Value, out int year))
		{
			return null;
		}
		if (!int.TryParse(dateElement.Element(XName.Get("month", xmlns))?.Value, out int month))
		{
			return new DateTime(year, 1, 1);
		}
		if (!int.TryParse(dateElement.Element(XName.Get("day", xmlns))?.Value, out int day))
		{
			return new DateTime(year, month, 1);
		}
		return new DateTime(year, month, day);
	}

	public static IEnumerable<ImportRecord> ParseDaWinci(XDocument doc)
	{
		//XDocument doc = XDocument.Parse(stringDoc);
		string marcns = "http://www.loc.gov/MARC21/slim";

		List<ImportRecord> result = new();

		var allRecords = doc.Descendants(XName.Get("record", marcns));

		foreach (XElement node in allRecords)
		{
			var parsedRecord = ParseDaWinciImportRecord(node, marcns);
			if (parsedRecord != null)
			{
				result.Add(parsedRecord);
			}
		}
		return result;
	}

	private static ImportRecord? ParseDaWinciImportRecord(XElement record, string marcns)
	{
		var importedRecord = new ImportRecord();
		var versionIdentifierElement = (from element in record.Elements()
								 where element.Attribute(DAWINCI_TAG)?.Value == "005"
								 select element).FirstOrDefault();
		if (versionIdentifierElement != null)
		{
			string version = versionIdentifierElement.Value;
			int year = int.Parse(version.AsSpan(0, 4));
			int month = int.Parse(version.AsSpan(4, 2));
			int day = int.Parse(version.AsSpan(6, 2));
			importedRecord.RecordVersionDate = new DateTime(year, month, day);
		}

		importedRecord.Publication = ParseDaWinciPublication(record, marcns);

		return importedRecord;
	}
}

