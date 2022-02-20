using System.Xml.Linq;
using ZUEPC.Import.Import.Models;
using static ZUEPC.Import.Import.Models.ImportInstitution;

namespace ZUEPC.Import.Import.Service;

partial class ImportService
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

		foreach(var nameElement in nameElements)
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
		var identifierElements = institutionElement.Element(XName.Get("institution_identifier", xmlns))?
			.Elements(XName.Get("local_numbers", xmlns));
		result.Add(externDbId);
		if(identifierElements is null)
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
}
