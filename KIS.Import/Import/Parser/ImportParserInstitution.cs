using System.Xml.Linq;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportInstitution;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	public static ImportInstitution ParseCREPCInstitution(XElement institutionElement, string xmlns)
	{
		ImportInstitution importInstitution = new();
		importInstitution.Level = ParseInt(institutionElement.Attribute("level")?.Value);
		importInstitution.InstitutionNames = ParseCREPCInstitutionNames(institutionElement, xmlns);
		importInstitution.InstitutionExternDbIds = ParseCREPCInstitutionExternDbId(institutionElement, xmlns);
		importInstitution.InstititutionType = institutionElement.Element(XName.Get("institution_type", xmlns))?.Value;
		return importInstitution;
	}

	public static List<ImportInstitutionName> ParseCREPCInstitutionNames(XElement institutionElement, string xmlns)
	{
		List<ImportInstitutionName> result = new();

		var nameElements = institutionElement.Elements(XName.Get("institution_name", xmlns));

		foreach (var nameElement in nameElements)
		{
			ImportInstitutionName institutionName = new()
			{
				Name = nameElement.Value,
				NameType = nameElement.Attribute("inst_type")?.Value
			};
			result.Add(institutionName);
		}
		return result;
	}

	public static List<ImportInstitutionExternDbId> ParseCREPCInstitutionExternDbId(XElement institutionElement, string xmlns)
	{
		List<ImportInstitutionExternDbId> result = new();

		string? input = institutionElement.Attribute("id")?.Value;
		var externDbId = new ImportInstitutionExternDbId()
		{
			InstitutionExternDbId = $"CREPC:{input}"
		};

		result.Add(externDbId);

		var insitutionTagElement = institutionElement.Element(XName.Get("institution_identifier", xmlns))?
								   .Element(XName.Get("institution_tag", xmlns));
		if(insitutionTagElement != null)
		{
			input = insitutionTagElement.Value;
			externDbId = new ImportInstitutionExternDbId()
			{
				InstitutionExternDbId = $"ins_tag:{input}"
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
			string? dbName = identifierElement.Element(XName.Get("num_title", xmlns))?.Value;
			string? idValue = identifierElement.Element(XName.Get("number", xmlns))?.Value;
			externDbId = new()
			{
				InstitutionExternDbId = $"{dbName}:{idValue}"
			};
			result.Add(externDbId);
		}
		return result;
	}

	public static ImportInstitution ParseDaWinciInstitution(XElement institutionElement, string xmlns)
	{
		ImportInstitution importInstitution = new();
		importInstitution.InstitutionNames = ParseDaWinciInstitutionNames(institutionElement, xmlns);
		importInstitution.InstitutionExternDbIds = ParseDaWinciInstitutionExternDbId(institutionElement, xmlns);
		importInstitution.InstititutionType = institutionElement.Element(XName.Get("institution_type", xmlns))?.Value;
		return importInstitution;
	}

	public static List<ImportInstitutionName> ParseDaWinciInstitutionNames(XElement institutionElement, string xmlns)
	{
		return new();
	}

	public static List<ImportInstitutionExternDbId> ParseDaWinciInstitutionExternDbId(XElement publicationElement, string xmlns)
	{
		List<ImportInstitutionExternDbId> result = new();
		var insitutionTagElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									where element.Attribute(DAWINCI_CODE)?.Value == "p"
									select element).FirstOrDefault();

		if (insitutionTagElement != null)
		{
			string input = insitutionTagElement.Value;
			var externDbId = new ImportInstitutionExternDbId()
			{
				InstitutionExternDbId = $"ins_tag:{input}"
			};
			result.Add(externDbId);
		}

		return result;
	}
}
