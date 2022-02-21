using System.Xml.Linq;

namespace ZUEPC.Import.Import.Service;

public partial class ImportParser
{
	public static void ManualImportCREPC(string stringDoc)
	{
		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\ADC_s_ohlasmi_my.xml");
		XDocument doc = XDocument.Parse(stringDoc);
		string biblibsearch = "http://biblib.net/search/";
		string xmlns = "http://www.crepc.sk/schema/xml-crepc2/";

		List<ImportRecord> result = new();
		var allRecords = doc.Descendants(XName.Get("record", biblibsearch));

		foreach (XElement node in allRecords)
		{
			result.Add(ParseCREPCImportRecord(node, biblibsearch, xmlns));
		}

		Console.WriteLine();
	}

	public static ImportRecord? ParseCREPCImportRecord(XElement record, string biblibsearch, string xmlns)
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

	public static int? ParseInt(string? value)
	{
		if (int.TryParse(value, out var result))
		{
			return result;
		}
		return null;
	}

	public static DateTime? ParseCREPCDate(XElement dateElement, string xmlns)
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
}

public class ImportRecord
{
	public DateTime RecordVersionDate { get; set; }
	public ImportPublication? Publication { get; set; }


	public string? RecordVersionDateString
	{
		set
		{
			if (value is null)
			{
				return;
			}
			if (!DateTime.TryParse(value, out var resultDate))
			{
				return;
			}
			RecordVersionDate = resultDate;
		}
	}
}