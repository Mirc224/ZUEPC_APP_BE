using System.Xml.Linq;
using ZUEPC.Import.Import.Models;
using static ZUEPC.Import.Import.Models.ImportPerson;

namespace ZUEPC.Import.Import.Service;

partial class ImportParser
{
	public static ImportPerson ParseCREPCPerson(XElement personElement, string xmlns)
	{
		ImportPerson person = new();
		person.PersonNames = ParseCREPCPersonNames(personElement, xmlns);
		person.PersonExternDbIds = ParseCREPCPersonExternDbIds(personElement, xmlns);

		var birthElement = (from element in personElement.Elements(XName.Get("periods", xmlns))
							where element.Attribute("birth_death")?.Value == "yes"
							select
							(from dateElement in element.Descendants(XName.Get("date", xmlns))
							 where dateElement.Element(XName.Get("year", xmlns))?.Attribute("period_type")?.Value == "from"
							 select dateElement).FirstOrDefault()).FirstOrDefault();
		if (birthElement != null)
		{
			person.BirthYear = ParseInt(birthElement.Element(XName.Get("year", xmlns))?.Value);
		}

		var deathElement = (from element in personElement.Elements(XName.Get("periods", xmlns))
							where element.Attribute("birth_death")?.Value == "yes"
							select
							(from dateElement in element.Descendants(XName.Get("date", xmlns))
							 where dateElement.Element(XName.Get("year", xmlns))?.Attribute("period_type")?.Value == "to"
							 select dateElement).FirstOrDefault()).FirstOrDefault();
		if (deathElement != null)
		{
			person.DeathYear = ParseInt(deathElement.Element(XName.Get("year", xmlns))?.Value);
		}

		return person;
	}

	public static List<ImportPersonExternDbId> ParseCREPCPersonExternDbIds(XElement personElement, string xmlns)
	{
		List<ImportPersonExternDbId> result = new();

		string? input = personElement.Attribute("id")?.Value;
		var externDbId = new ImportPersonExternDbId()
		{
			PersonExternDbId = "CREPC:" + input,
		};

		result.Add(externDbId);

		var records = personElement.Elements(XName.Get("cross_person_database", xmlns));

		foreach (var recordElement in records)
		{
			ImportPersonExternDbId personExterDbId = new();
			string? idLabel = recordElement.Attribute("id_title")?.Value;
			string? idValue = recordElement.Attribute("id_value")?.Value;
			personExterDbId.PersonExternDbId = $"{idLabel}:{idValue}";
			result.Add(personExterDbId);
		}
		return result;
	}

	public static List<ImportPersonName> ParseCREPCPersonNames(XElement personElement, string xmlns)
	{
		List<ImportPersonName> result = new();

		ImportPersonName personName = new()
		{
			NameType = personElement.Attribute("relationship")?.Value,
			FirstName = personElement.Element(XName.Get("firstname", xmlns))?.Value,
			LastName = personElement.Element(XName.Get("lastname", xmlns))?.Value
		};

		result.Add(personName);
		return result;
	}
}
