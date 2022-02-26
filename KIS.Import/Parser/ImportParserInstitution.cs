using System.Xml.Linq;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportInstitution;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static ImportInstitution ParseCREPCInstitution(XElement institutionElement, string xmlns)
	{
		ImportInstitution importInstitution = new();
		importInstitution.Level = ParseInt(institutionElement.Attribute("level")?.Value);
		importInstitution.InstitutionNames = ParseCREPCInstitutionNames(institutionElement, xmlns);
		importInstitution.InstitutionExternDbIds = ParseCREPCInstitutionExternDbId(institutionElement, xmlns);
		importInstitution.InstititutionType = institutionElement.Element(XName.Get("institution_type", xmlns))?.Value.Trim();
		return importInstitution;
	}

	private static List<ImportInstitutionName> ParseCREPCInstitutionNames(XElement institutionElement, string xmlns)
	{
		List<ImportInstitutionName> result = new();

		var nameElements = institutionElement.Elements(XName.Get("institution_name", xmlns));

		foreach (var nameElement in nameElements)
		{
			ImportInstitutionName institutionName = new()
			{
				Name = nameElement.Value.Trim(),
				NameType = nameElement.Attribute("inst_type")?.Value.Trim()
			};
			result.Add(institutionName);
		}
		return result;
	}

	private static List<ImportInstitutionExternDatabaseId> ParseCREPCInstitutionExternDbId(XElement institutionElement, string xmlns)
	{
		List<ImportInstitutionExternDatabaseId> result = new();

		string? input = institutionElement.Attribute("id")?.Value.Trim();
		var externDbId = new ImportInstitutionExternDatabaseId()
		{
			ExternIdentifierValue = $"CREPC:{input}"
		};

		result.Add(externDbId);

		var insitutionTagElement = institutionElement.Element(XName.Get("institution_identifier", xmlns))?
								   .Element(XName.Get("institution_tag", xmlns));
		if(insitutionTagElement != null)
		{
			input = insitutionTagElement.Value;
			externDbId = new ImportInstitutionExternDatabaseId()
			{
				ExternIdentifierValue = $"ins_tag:{input}"
			};
			result.Add(externDbId);
		}

		var identifierElements = institutionElement.Element(XName.Get("institution_identifier", xmlns))?
			.Elements(XName.Get("local_numbers", xmlns));

		if (identifierElements is null)
		{
			return result;
		}
		foreach (var identifierElement in identifierElements)
		{
			string? dbName = identifierElement.Element(XName.Get("num_title", xmlns))?.Value.Trim();
			string? idValue = identifierElement.Element(XName.Get("number", xmlns))?.Value.Trim();
			externDbId = new()
			{
				ExternIdentifierValue = $"{dbName}:{idValue}"
			};
			result.Add(externDbId);
		}
		return result;
	}

	private static ImportInstitution ParseDaWinciInstitution(XElement institutionElement, string xmlns)
	{
		ImportInstitution importInstitution = new();
		importInstitution.InstitutionNames = ParseDaWinciInstitutionNames(institutionElement, xmlns);
		importInstitution.InstitutionExternDbIds = ParseDaWinciInstitutionExternDbId(institutionElement, xmlns);
		importInstitution.InstititutionType = institutionElement.Element(XName.Get("institution_type", xmlns))?.Value.Trim();
		return importInstitution;
	}

	private static List<ImportInstitutionName> ParseDaWinciInstitutionNames(XElement institutionElement, string xmlns)
	{
		return new();
	}

	private static List<ImportInstitutionExternDatabaseId> ParseDaWinciInstitutionExternDbId(XElement publicationElement, string xmlns)
	{
		List<ImportInstitutionExternDatabaseId> result = new();
		var insitutionTagElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									where element.Attribute(DAWINCI_CODE)?.Value == "p"
									select element).FirstOrDefault();

		if (insitutionTagElement != null)
		{
			string input = insitutionTagElement.Value.Trim();
			var externDbId = new ImportInstitutionExternDatabaseId()
			{
				ExternIdentifierValue = $"ins_tag:{input}"
			};
			result.Add(externDbId);
		}

		return result;
	}
}
