using System.Xml.Linq;
using ZUEPC.Base.Extensions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPerson;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static ImportPerson ParseCREPCPerson(XElement personElement, string xmlns)
	{
		ImportPerson person = new();
		person.PersonNames = ParseCREPCPersonNames(personElement, xmlns);
		person.PersonExternDatabaseIds = ParseCREPCPersonExternDbIds(personElement, xmlns);

		XElement? birthElement = (from element in personElement.Elements(XName.Get("periods", xmlns))
								  where element.Attribute("birth_death")?.Value == "yes"
								  select
								  (from dateElement in element.Descendants(XName.Get("date", xmlns))
								   where dateElement.Element(XName.Get("year", xmlns))?.Attribute("period_type")?.Value == "from"
								   select dateElement).FirstOrDefault()).FirstOrDefault();
		if (birthElement != null)
		{
			person.BirthYear = ParseInt(birthElement.Element(XName.Get("year", xmlns))?.Value);
		}

		XElement? deathElement = (from element in personElement.Elements(XName.Get("periods", xmlns))
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

	private static List<ImportPersonExternDatabaseId> ParseCREPCPersonExternDbIds(XElement personElement, string xmlns)
	{
		List<ImportPersonExternDatabaseId> result = new();

		string? input = personElement.Attribute("id")?.Value.Trim();
		ImportPersonExternDatabaseId? externDbId = new()
		{
			ExternIdentifierValue = $"{CREPC_IDENTIFIER_PREFIX}:{input}",
		};

		result.Add(externDbId);

		IEnumerable<XElement>? records = personElement.Elements(XName.Get("cross_person_database", xmlns));

		foreach (XElement? recordElement in records.OrEmptyIfNull())
		{
			ImportPersonExternDatabaseId personExterDbId = new();
			string? idLabel = recordElement.Attribute("id_title")?.Value.Trim();
			string? idValue = recordElement.Attribute("id_value")?.Value.Trim();
			personExterDbId.ExternIdentifierValue = $"{idLabel}:{idValue}";
			result.Add(personExterDbId);
		}
		return result;
	}

	private static List<ImportPersonName> ParseCREPCPersonNames(XElement personElement, string xmlns)
	{
		List<ImportPersonName> result = new();

		ImportPersonName personName = new()
		{
			NameType = personElement.Attribute("relationship")?.Value.Trim(),
			FirstName = personElement.Element(XName.Get("firstname", xmlns))?.Value.Trim(),
			LastName = personElement.Element(XName.Get("lastname", xmlns))?.Value.Trim()
		};

		result.Add(personName);
		return result;
	}

	private static ImportPerson ParseDaWinciPerson(XElement personElement, string xmlns)
	{
		ImportPerson person = new();
		person.PersonNames = ParseDaWinciPersonNames(personElement, xmlns);
		person.PersonExternDatabaseIds = ParseDaWinciPersonExternDbIds(personElement, xmlns);

		XElement? birthDeathElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									   where element.Attribute(DAWINCI_CODE)?.Value == "f"
									   select element).FirstOrDefault();

		if (birthDeathElement != null)
		{
			string? intervalString = birthDeathElement.Value.Trim();
			string[]? yearsArr = intervalString.Split('-');
			if (!string.IsNullOrEmpty(yearsArr[0]))
			{
				person.BirthYear = ParseInt(yearsArr[0]);
			}

			if (yearsArr.Length > 1 && !string.IsNullOrEmpty(yearsArr[0]))
			{
				person.DeathYear = ParseInt(yearsArr[1]);
			}
		}

		return person;
	}

	private static List<ImportPersonName> ParseDaWinciPersonNames(XElement personElement, string xmlns)
	{
		List<ImportPersonName> result = new();
		string? nameType = "real_name";
		string? firstName = null;
		string? lastName = null;

		XElement? nameElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
								 where element.Attribute(DAWINCI_CODE)?.Value == "b"
								 select element).FirstOrDefault();
		if (nameElement != null)
		{
			firstName = nameElement.Value.Trim();
		}

		nameElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
					   where element.Attribute(DAWINCI_CODE)?.Value == "a"
					   select element).FirstOrDefault();
		if (nameElement != null)
		{
			lastName = nameElement.Value.Trim();
		}

		ImportPersonName personName = new()
		{
			NameType = nameType,
			FirstName = firstName,
			LastName = lastName,
		};

		result.Add(personName);
		return result;
	}

	private static List<ImportPersonExternDatabaseId> ParseDaWinciPersonExternDbIds(XElement personElement, string xmlns)
	{
		List<ImportPersonExternDatabaseId> result = new();

		XElement? personIdElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									 where element.Attribute(DAWINCI_CODE)?.Value == "3"
									 select element).FirstOrDefault();
		if (personIdElement != null)
		{
			ImportPersonExternDatabaseId externDbId = new()
			{
				ExternIdentifierValue = $"{ZU_PERSONID_PREFIX}:{personIdElement?.Value.Trim()}"
			};
			result.Add(externDbId);
		}
		return result;
	}
}
