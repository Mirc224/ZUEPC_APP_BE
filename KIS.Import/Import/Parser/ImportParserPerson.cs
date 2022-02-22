using System.Xml.Linq;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPerson;

namespace ZUEPC.Import.Parser;

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
			PersonExternDbId = $"{CREPC_IDENTIFIER_PREFIX}:{input}",
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

	public static ImportPerson ParseDaWinciPerson(XElement personElement, string xmlns)
	{
		ImportPerson person = new();
		person.PersonNames = ParseDaWinciPersonNames(personElement, xmlns);
		person.PersonExternDbIds = ParseDaWinciPersonExternDbIds(personElement, xmlns);

		var birthDeathElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
								 where element.Attribute(DAWINCI_CODE)?.Value == "f"
								 select element).FirstOrDefault();

		if (birthDeathElement != null)
		{
			var intervalString = birthDeathElement.Value.Trim();
			var yearsArr = intervalString.Split('-');
			if(!string.IsNullOrEmpty(yearsArr[0]))
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

	public static List<ImportPersonName> ParseDaWinciPersonNames(XElement personElement, string xmlns)
	{
		List<ImportPersonName> result = new();
		string? nameType = "real_name";
		string? firstName = null;
		string? lastName = null;

		var nameElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
						  where element.Attribute(DAWINCI_CODE)?.Value == "a"
						  select element).FirstOrDefault();
		if(nameElement != null)
		{
			firstName = nameElement.Value;
		}

		nameElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
						   where element.Attribute(DAWINCI_CODE)?.Value == "b"
						   select element).FirstOrDefault();
		if (nameElement != null)
		{
			lastName = nameElement.Value;
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

	public static List<ImportPersonExternDbId> ParseDaWinciPersonExternDbIds(XElement personElement, string xmlns)
	{
		List<ImportPersonExternDbId> result = new();

		var personIdElement = (from element in personElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
							   where element.Attribute(DAWINCI_CODE)?.Value == "3"
							   select element).FirstOrDefault();
		if (personIdElement != null)
		{
			ImportPersonExternDbId externDbId = new()
			{
				PersonExternDbId = $"{ZU_PERSONID_PREFIX}:{personIdElement?.Value}"
			};
			result.Add(externDbId);
		}
		return result;
	}
}
