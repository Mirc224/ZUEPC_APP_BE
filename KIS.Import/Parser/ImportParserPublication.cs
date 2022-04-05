using System.Xml.Linq;
using ZUEPC.Base.Extensions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static ImportPublication ParseCREPCPublication(XElement publicationElement, string xmlns)
	{
		ImportPublication importedPublication = new()
		{
			PublicationTypeAsString = publicationElement.Attribute("form_type")?.Value,
			PublicationExternDbIds = ParseCREPCPublicationExternDbIdentifiers(publicationElement, xmlns),
			PublicationIdentifiers = ParseCREPCPublicationIdentifiers(publicationElement, xmlns),
			PublicationNames = ParseCREPCPublicationNames(publicationElement, xmlns),
			PublicationAuthors = ParseCREPCPublicationAuthors(publicationElement, xmlns),
			RelatedPublications = ParseCREPCRelatedPublications(publicationElement, xmlns),
			PublicationActivities = ParseCREPCPublishingActivityDetails(publicationElement, xmlns),
			PublishYear = ParseCREPCPublicationPubishYear(publicationElement, xmlns)
		};

		foreach (ImportPublicationActivity? publishActivity in importedPublication.PublicationActivities.OrEmptyIfNull())
		{
			publishActivity.ActivityYear = importedPublication.PublishYear;
		}

		XElement? documentTypeElement = publicationElement.Element(XName.Get("document_type", xmlns));
		if (documentTypeElement != null)
		{
			importedPublication.DocumentType = documentTypeElement.Value.Trim();
		}

		return importedPublication;
	}

	private static int? ParseCREPCPublicationPubishYear(XElement publicationElement, string xmlns)
	{
		int? publishYear = null;
		XElement? biblioYearElement = (from element in publicationElement.Elements(XName.Get("biblio_year", xmlns))
									   where element.Attribute("type")?.Value == "published"
									   select (from dateElement in element.Elements(XName.Get("date", xmlns))
											   select (from yearElement in dateElement.Elements(XName.Get("year", xmlns))
													   where yearElement.Attribute("period_type")?.Value == "from"
													   select dateElement).FirstOrDefault()
											   ).FirstOrDefault()
								 ).FirstOrDefault();
		if (biblioYearElement != null)
		{
			publishYear = ParseInt(biblioYearElement.Value);
			return publishYear;
		}

		XElement? crossSourceElement = (from crossPublEle in publicationElement.Elements(XName.Get("cross_biblio_biblio", xmlns))
										where crossPublEle.Attribute("source")?.Value == "source"
										select crossPublEle).FirstOrDefault();

		if (crossSourceElement is null)
		{
			return publishYear;
		}

		XElement? sourcePublElement = (from publElement in crossSourceElement.Elements(XName.Get("rec_biblio", xmlns))
									   select publElement).FirstOrDefault();

		if (sourcePublElement is null)
		{
			return publishYear;
		}

		biblioYearElement = (from element in sourcePublElement.Elements(XName.Get("biblio_year", xmlns))
							 where element.Attribute("type")?.Value == "published"
							 select (from dateElement in element.Elements(XName.Get("date", xmlns))
									 select (from yearElement in dateElement.Elements(XName.Get("year", xmlns))
											 where yearElement.Attribute("period_type")?.Value == "from"
											 select dateElement
											).FirstOrDefault()
									).FirstOrDefault()
							).FirstOrDefault();


		if (biblioYearElement != null)
		{
			publishYear = ParseInt(biblioYearElement.Value);
			return publishYear;
		}

		biblioYearElement = (from issueElement in crossSourceElement.Elements(XName.Get("rec_issue", xmlns))
							 select (from dateElement in issueElement.Elements(XName.Get("date", xmlns))
									 select dateElement.Element(XName.Get("year", xmlns))).FirstOrDefault()
							 ).FirstOrDefault();

		if (biblioYearElement != null)
		{
			publishYear = ParseInt(biblioYearElement.Value);
			return publishYear;
		}


		return publishYear;
	}

	private static List<ImportPublicationIdentifier> ParseCREPCPublicationIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();

		IEnumerable<XElement>? records = publicationElement.Element(XName.Get("biblio_identifier", xmlns))?.Elements(XName.Get("digi_identifier", xmlns));
		foreach (XElement? recordElement in records.OrEmptyIfNull())
		{
			ImportPublicationIdentifier? publicationId = PublicationIdCreator("DOI", recordElement.Value.Trim());
			if (publicationId != null)
			{
				result.Add(publicationId);
			}
		}

		records = publicationElement.Element(XName.Get("biblio_identifier", xmlns))?.Elements(XName.Get("int_standards", xmlns));

		foreach (XElement? recordElement in records.OrEmptyIfNull())
		{
			string? isName = recordElement.Attribute("is_type")?.Value.Trim();
			string? isForm = recordElement.Attribute("is_form")?.Value.Trim();
			ImportPublicationIdentifier? publicationId = PublicationIdCreator(isName, recordElement.Value.Trim(), isForm);
			if (publicationId != null)
			{
				result.Add(publicationId);
			}
		}

		return result;
	}

	private static List<ImportPublicationExternDatabaseId> ParseCREPCPublicationExternDbIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationExternDatabaseId> result = new();
		string? input = publicationElement.Attribute("id")?.Value.Trim();
		ImportPublicationExternDatabaseId? externDbId = new()
		{
			ExternIdentifierValue = $"{CREPC_IDENTIFIER_PREFIX}:{input}",
		};

		result.Add(externDbId);

		IEnumerable<XElement>? records = publicationElement.Elements(XName.Get("cross_biblio_database", xmlns));

		if (!records.Any())
		{
			return result;
		}

		foreach (XElement recordElement in records.OrEmptyIfNull())
		{
			string? idValue = recordElement.Element(XName.Get("database_id", xmlns))?.Value.Trim();
			if (idValue is null ||
			   idValue == "wos" ||
			   idValue == "scopus" ||
			   idValue == "ccc")
			{
				continue;
			}
			ImportPublicationExternDatabaseId publicationExterDbId = new();
			publicationExterDbId.ExternIdentifierValue = idValue;
			result.Add(publicationExterDbId);
		}

		return result;
	}

	private static List<ImportPublicationName> ParseCREPCPublicationNames(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationName> result = new();

		IEnumerable<XElement>? publicationNames = publicationElement.Elements(XName.Get("title", xmlns));
		foreach (XElement? recordElement in publicationNames.OrEmptyIfNull())
		{
			string? name = recordElement.Value;
			string? nameType = recordElement.Attribute("title_type")?.Value.Trim();
			ImportPublicationName publicationNameDetails = new()
			{
				Name = name,
				NameType = nameType
			};
			result.Add(publicationNameDetails);
		}

		return result;
	}

	private static ImportPublicationIdentifier? PublicationIdCreator(string? identifierName, string? idValue, string? ISForm = null)
	{
		if (idValue is null || identifierName is null)
		{
			return null;
		}
		return new ImportPublicationIdentifier
		{
			IdentifierName = identifierName,
			IdentifierValue = idValue,
			ISForm = ISForm
		};
	}

	private static ImportPublication ParseDaWinciPublication(XElement publicationElement, string xmlns)
	{
		ImportPublication importedPublication = new()
		{
			PublicationExternDbIds = ParseDaWinciPublicationExternDbIdentifiers(publicationElement, xmlns),

			PublicationIdentifiers = ParseDaWinciPublicationIdentifiers(publicationElement, xmlns),
			PublicationNames = ParseDaWinciPublicationNames(publicationElement, xmlns),
			PublicationAuthors = ParseDaWinciPublicationAuthors(publicationElement, xmlns),
			RelatedPublications = ParseDaWinciRelatedPublications(publicationElement, xmlns),
			PublicationActivities = ParseDaWinciPublishingActivityDetails(publicationElement, xmlns),
			PublishYear = ParseDaWinciPublicationPubishYear(publicationElement, xmlns)
		};

		XElement? documentTypeElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										 where element.Attribute(DAWINCI_TAG)?.Value == "992"
										 select (from documentTypeElement in element.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
												 where documentTypeElement.Attribute(DAWINCI_CODE)?.Value == "a"
												 select documentTypeElement).FirstOrDefault()
										   ).FirstOrDefault();
		if (documentTypeElement != null)
		{
			importedPublication.DocumentType = documentTypeElement.Value.Trim();
		}

		return importedPublication;
	}

	private static int? ParseDaWinciPublicationPubishYear(XElement publicationElement, string xmlns)
	{
		int? publishYear = null;

		XElement? publishYearElement = (from publishingDetailsElement in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										where publishingDetailsElement.Attribute(DAWINCI_TAG)?.Value == "210"
										select (from subfieldElement in publishingDetailsElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
												where subfieldElement.Attribute(DAWINCI_CODE)?.Value == "d"
												select subfieldElement).FirstOrDefault()
										  ).FirstOrDefault();
		if (publishYearElement != null)
		{
			publishYear = ParseInt(publishYearElement.Value);
		}

		return publishYear;
	}

	private static List<ImportPublicationExternDatabaseId> ParseDaWinciPublicationExternDbIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationExternDatabaseId> result = new();
		string? input = (from element in publicationElement.Elements(XName.Get(DAWINCI_CONTROLFIELD, xmlns))
						 where element.Attribute(DAWINCI_TAG)?.Value == "001"
						 select element?.Value).FirstOrDefault();

		string systemName = ZU_PUBLICATIONID_PREFIX + ".";
		if (input != null && input.StartsWith("CREPC"))
		{
			systemName = CREPC_IDENTIFIER_PREFIX + ":";
			input = input.Substring(5);
		}

		if (input != null)
		{
			ImportPublicationExternDatabaseId? externDbId = new()
			{
				ExternIdentifierValue = $"{systemName}{input}",
			};
			result.Add(externDbId);
		}

		IEnumerable<XElement>? records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										 where element.Attribute(DAWINCI_TAG)?.Value == "RID"
										 select element;

		foreach (XElement recordElement in records.OrEmptyIfNull())
		{
			XElement? idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										   where element.Attribute(DAWINCI_CODE)?.Value == "a"
										   select element).FirstOrDefault();

			if (idSubfieldElement is null)
			{
				continue;
			}

			ImportPublicationExternDatabaseId publicationExterDbId = new();
			publicationExterDbId.ExternIdentifierValue = idSubfieldElement?.Value;
			result.Add(publicationExterDbId);
		}

		records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
				  where element.Attribute(DAWINCI_TAG)?.Value == "985"
				  select element;


		foreach (XElement recordElement in records.OrEmptyIfNull())
		{
			XElement? idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										   where element.Attribute(DAWINCI_CODE)?.Value == "3"
										   select element).FirstOrDefault();

			if (idSubfieldElement is null)
			{
				continue;
			}

			ImportPublicationExternDatabaseId publicationExterDbId = new();
			input = idSubfieldElement?.Value;

			if (input is null)
			{
				continue;
			}
			publicationExterDbId.ExternIdentifierValue = $"{CREPC_IDENTIFIER_PREFIX}:{input}";
			result.Add(publicationExterDbId);
		}

		return result;
	}

	private static List<ImportPublicationIdentifier> ParseDaWinciPublicationIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();

		string? input = null;
		IEnumerable<XElement>? records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										 where element.Attribute(DAWINCI_TAG)?.Value == "913"
										 select element;

		foreach (XElement recordElement in records.OrEmptyIfNull())
		{
			XElement? idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										   where element.Attribute(DAWINCI_CODE)?.Value == "a"
										   select element).FirstOrDefault();
			if (idSubfieldElement is null)
			{
				continue;
			}

			input = idSubfieldElement?.Value.Trim();
			if (input is null)
			{
				continue;
			}

			if (input.StartsWith("DOI"))
			{
				string[]? inputArray = input.Split(' ');
				input = inputArray[1];
			}

			ImportPublicationIdentifier? publicationId = PublicationIdCreator("DOI", input);
			if (publicationId != null)
			{
				result.Add(publicationId);
			}
		}


		records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
				  where element.Attribute(DAWINCI_TAG)?.Value == "010" ||
						element.Attribute(DAWINCI_TAG)?.Value == "011" ||
						element.Attribute(DAWINCI_TAG)?.Value == "013" ||
						element.Attribute(DAWINCI_TAG)?.Value == "015" ||
						element.Attribute(DAWINCI_TAG)?.Value == "016"
				  select element;

		List<ImportPublicationIdentifier>? standardIdentifiers = ParseDaWinciPublicationStandardIdentifiers(records, xmlns);
		result.AddRange(standardIdentifiers);

		return result;
	}

	private static List<ImportPublicationName> ParseDaWinciPublicationNames(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationName> result = new();

		IEnumerable<XElement>? publicationDetailsElements = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
															where element.Attribute(DAWINCI_TAG)?.Value == "200"
															select element;

		List<ImportPublicationName>? publicationNames = ParseDaWinciPublicationNamesFromElements(publicationDetailsElements, xmlns);
		result.AddRange(publicationNames);

		return result;
	}

	private static ImportPublication ParseDaWinciSourcePublication(XElement sourcePublicationElement, string xmlns)
	{
		ImportPublication result = new();

		result.PublicationIdentifiers = ParseDaWinciSourcePublicationIdentifiers(sourcePublicationElement, xmlns);
		result.PublicationExternDbIds = ParseDaWinciSourcePublicationExternDbIds(sourcePublicationElement, xmlns);
		result.PublicationNames = ParseDaWinciSourcePublicationNames(sourcePublicationElement, xmlns);

		return result;
	}

	private static List<ImportPublicationName> ParseDaWinciSourcePublicationNames(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationName> result = new();

		IEnumerable<XElement>? publicationDetailsElements = from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_DATAFIELD, xmlns))
															where element.Attribute(DAWINCI_TAG)?.Value == "200"
															select element;

		List<ImportPublicationName>? publicationNames = ParseDaWinciPublicationNamesFromElements(publicationDetailsElements, xmlns);
		result.AddRange(publicationNames);
		return result;
	}

	private static List<ImportPublicationIdentifier> ParseDaWinciSourcePublicationIdentifiers(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();
		IEnumerable<XElement>? identifierElements = (from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_DATAFIELD, xmlns))
													 where element.Attribute(DAWINCI_TAG)?.Value == "010" ||
													 element.Attribute(DAWINCI_TAG)?.Value == "011" ||
													 element.Attribute(DAWINCI_TAG)?.Value == "013" ||
													 element.Attribute(DAWINCI_TAG)?.Value == "015" ||
													 element.Attribute(DAWINCI_TAG)?.Value == "016"
													 select element);

		List<ImportPublicationIdentifier>? standardIdentifiers = ParseDaWinciPublicationStandardIdentifiers(identifierElements, xmlns);
		result.AddRange(standardIdentifiers);

		return result;
	}

	private static List<ImportPublicationExternDatabaseId> ParseDaWinciSourcePublicationExternDbIds(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationExternDatabaseId> result = new();

		XElement? identifierElement = (from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_CONTROLFIELD, xmlns))
									   where element.Attribute(DAWINCI_TAG)?.Value == "001"
									   select element).FirstOrDefault();
		if (identifierElement is null)
		{
			return result;
		}

		string input = identifierElement.Value.Trim();
		string systemName = ZU_PUBLICATIONID_PREFIX + ".";
		if (input.StartsWith("CREPC"))
		{
			systemName = CREPC_IDENTIFIER_PREFIX + ":";
			input = input.Substring(5);
		}
		ImportPublicationExternDatabaseId newIdentifier = new()
		{
			ExternIdentifierValue = $"{systemName}{input}"
		};
		result.Add(newIdentifier);
		return result;
	}


	private static List<ImportPublicationName> ParseDaWinciPublicationNamesFromElements(
		IEnumerable<XElement> publicationDetailsElement,
		string xmlns)
	{
		List<ImportPublicationName> result = new();

		foreach (XElement detailElement in publicationDetailsElement.OrEmptyIfNull())
		{
			XElement? nameElement = (from element in detailElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									 where element.Attribute(DAWINCI_CODE)?.Value == "a"
									 select element).FirstOrDefault();
			if (nameElement is null)
			{
				continue;
			}

			string? name = nameElement.Value.Trim();
			ImportPublicationName? publicationNameDetails = new ImportPublicationName
			{
				Name = name,
				NameType = "title_proper"
			};
			result.Add(publicationNameDetails);
		}
		return result;
	}

	private static List<ImportPublicationIdentifier> ParseDaWinciPublicationStandardIdentifiers(
		IEnumerable<XElement> records,
		string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();
		Dictionary<string, string>? intStandardTypes = new()
		{
			{ "010", "isbn" },
			{ "011", "issn" },
			{ "013", "ismn" },
			{ "015", "isrn" },
			{ "016", "isrc" }
		};

		foreach (XElement? recordElement in records.OrEmptyIfNull())
		{
			string? tag = recordElement.Attribute(DAWINCI_TAG)?.Value;
			if (tag is null || !intStandardTypes.TryGetValue(tag, out string? isName))
			{
				continue;
			}
			string? idValue = recordElement.Value.Trim();

			string[]? inputArr = idValue.Split(' ');
			string? isForm = null;
			if (inputArr.Length > 1)
			{
				idValue = inputArr[0];
				isForm = inputArr[1];
			}
			ImportPublicationIdentifier? publicationId = PublicationIdCreator(isName, idValue, isForm);
			if (publicationId != null)
			{
				result.Add(publicationId);
			}
		}
		return result;
	}

}
