﻿using System.Xml.Linq;
using ZUEPC.Import.Import.Models;
using static ZUEPC.Import.Import.Models.ImportExternDatabase;
using static ZUEPC.Import.Import.Service.ImportPublication;

namespace ZUEPC.Import.Import.Service;

partial class ImportParser
{
	public static ImportPublication ParseCREPCPublication(XElement publicationElement, string xmlns)
	{
		var importedPublication = new ImportPublication();
		importedPublication.PublicationTypeAsString = publicationElement.Attribute("form_type")?.Value;

		importedPublication.PublicationExternDbIds = ParseCREPCPublicationExternDbIdentifiers(publicationElement, xmlns);
		importedPublication.PublicationIds = ParseCREPCPublicationIdentifiers(publicationElement, xmlns);
		importedPublication.PublicationNames = ParseCREPCPublicationNames(publicationElement, xmlns);
		importedPublication.PublicationAuthors = ParseCREPCPublicationAuthors(publicationElement, xmlns);
		importedPublication.RelatedPublications = ParseCREPCRelatedPublications(publicationElement, xmlns);
		importedPublication.PublishingActivities = ParseCREPCPublishingActivityDetails(publicationElement, xmlns);

		var documentTypeElement = publicationElement.Element(XName.Get("document_type", xmlns));
		if (documentTypeElement != null)
		{
			importedPublication.DocumentType = documentTypeElement.Value;
		}

		return importedPublication;
	}

	public static List<ImportPublicationIdentifier> ParseCREPCPublicationIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();

		var records = publicationElement.Element(XName.Get("biblio_identifier", xmlns))?.Elements(XName.Get("digi_identifier", xmlns));
		if (records != null)
		{
			foreach (var recordElement in records)
			{
				var publicationId = PublicationIdCreator("DOI", recordElement.Value);
				if (publicationId != null)
				{
					result.Add(publicationId);
				}
			}
		}

		records = publicationElement.Element(XName.Get("biblio_identifier", xmlns))?.Elements(XName.Get("int_standards", xmlns));

		if (records != null)
		{
			foreach (var recordElement in records)
			{
				var isName = recordElement.Attribute("is_type")?.Value;
				var isForm = recordElement.Attribute("is_form")?.Value;
				var publicationId = PublicationIdCreator(isName, recordElement.Value, isForm);
				if (publicationId != null)
				{
					result.Add(publicationId);
				}
			}
		}

		return result;
	}

	public static List<ImportPublicationExternDbId> ParseCREPCPublicationExternDbIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationExternDbId> result = new();
		string? input = publicationElement.Attribute("id")?.Value;
		var externDbId = new ImportPublicationExternDbId()
		{
			PublicationId = $"{CREPC_IDENTIFIER_PREFIX}:{input}",
		};

		result.Add(externDbId);

		var records = publicationElement.Elements(XName.Get("cross_biblio_database", xmlns));

		if (!records.Any())
		{
			return result;
		}

		foreach (var recordElement in records)
		{
			ImportPublicationExternDbId publicationExterDbId = new();
			publicationExterDbId.PublicationId = recordElement.Element(XName.Get("database_id", xmlns))?.Value;
			result.Add(publicationExterDbId);
		}

		return result;
	}

	public static List<ImportPublicationNameDetails> ParseCREPCPublicationNames(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationNameDetails> result = new();

		var publicationNames = publicationElement.Elements(XName.Get("title", xmlns));
		foreach (var recordElement in publicationNames)
		{
			var name = recordElement.Value;
			var nameType = recordElement.Attribute("title_type")?.Value;
			var publicationNameDetails = new ImportPublicationNameDetails
			{
				Name = name,
				NameType = nameType
			};
			result.Add(publicationNameDetails);
		}

		return result;
	}

	public static ImportPublicationIdentifier? PublicationIdCreator(string? identifierName, string? idValue, string? is_form = null)
	{
		if (idValue is null || identifierName is null)
		{
			return null;
		}
		return new ImportPublicationIdentifier
		{
			IdentifierName = identifierName,
			PublicationId = idValue,
			ISForm = is_form
		};
	}

	public static ImportPublication ParseDaWinciPublication(XElement publicationElement, string xmlns)
	{
		var importedPublication = new ImportPublication();
		importedPublication.PublicationTypeAsString = publicationElement.Attribute("form_type")?.Value;

		importedPublication.PublicationExternDbIds = ParseDaWinciPublicationExternDbIdentifiers(publicationElement, xmlns);

		importedPublication.PublicationIds = ParseDaWinciPublicationIdentifiers(publicationElement, xmlns);
		importedPublication.PublicationNames = ParseDaWinciPublicationNames(publicationElement, xmlns);
		importedPublication.PublicationAuthors = ParseDaWinciPublicationAuthors(publicationElement, xmlns);
		importedPublication.RelatedPublications = ParseDaWinciRelatedPublications(publicationElement, xmlns);
		importedPublication.PublishingActivities = ParseDaWinciPublishingActivityDetails(publicationElement, xmlns);

		var documentTypeElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
								   where element.Attribute(DAWINCI_TAG)?.Value == "992"
								   select (from documentTypeElement in element.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										   where documentTypeElement.Attribute(DAWINCI_CODE)?.Value == "a"
										   select documentTypeElement).FirstOrDefault()
										   ).FirstOrDefault();
		if (documentTypeElement != null)
		{
			importedPublication.DocumentType = documentTypeElement.Value;
		}

		return importedPublication;
	}

	public static List<ImportPublicationExternDbId> ParseDaWinciPublicationExternDbIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationExternDbId> result = new();
		string? input = (from element in publicationElement.Elements(XName.Get(DAWINCI_CONTROLFIELD, xmlns))
						 where element.Attribute(DAWINCI_TAG)?.Value == "001"
						 select element?.Value).FirstOrDefault();
		var externDbId = new ImportPublicationExternDbId()
		{
			PublicationId = $"{ZU_PUBLICATIONID_PREFIX}:{input}",
		};
		result.Add(externDbId);

		var records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
					  where element.Attribute(DAWINCI_TAG)?.Value == "RID"
					  select element;

		if (records.Any())
		{
			foreach (var recordElement in records)
			{
				var idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										 where element.Attribute(DAWINCI_CODE)?.Value == "a"
										 select element).FirstOrDefault();

				if (idSubfieldElement is null)
				{
					continue;
				}

				ImportPublicationExternDbId publicationExterDbId = new();
				publicationExterDbId.PublicationId = idSubfieldElement?.Value;
				result.Add(publicationExterDbId);
			}
		}

		records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
				  where element.Attribute(DAWINCI_TAG)?.Value == "985"
				  select element;

		if (records.Any())
		{
			foreach (var recordElement in records)
			{
				var idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										 where element.Attribute(DAWINCI_CODE)?.Value == "3"
										 select element).FirstOrDefault();

				if (idSubfieldElement is null)
				{
					continue;
				}

				ImportPublicationExternDbId publicationExterDbId = new();
				input = idSubfieldElement?.Value;

				if (input is null)
				{
					continue;
				}
				publicationExterDbId.PublicationId = $"{CREPC_IDENTIFIER_PREFIX}:{input}";
				result.Add(publicationExterDbId);
			}
		}

		return result;
	}

	public static List<ImportPublicationIdentifier> ParseDaWinciPublicationIdentifiers(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();

		string? input = null;
		var records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
					  where element.Attribute(DAWINCI_TAG)?.Value == "913"
					  select element;
		if (records != null)
		{
			foreach (var recordElement in records)
			{
				var idSubfieldElement = (from element in recordElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
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
					var inputArray = input.Split(' ');
					input = inputArray[1];
				}

				var publicationId = PublicationIdCreator("DOI", input);
				if (publicationId != null)
				{
					result.Add(publicationId);
				}
			}
		}


		records = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
				  where element.Attribute(DAWINCI_TAG)?.Value == "010" ||
						element.Attribute(DAWINCI_TAG)?.Value == "011" ||
						element.Attribute(DAWINCI_TAG)?.Value == "013" ||
						element.Attribute(DAWINCI_TAG)?.Value == "015" ||
						element.Attribute(DAWINCI_TAG)?.Value == "016"
				  select element;

		var standardIdentifiers = ParseDaWinciPublicationStandardIdentifiers(records, xmlns);
		result.AddRange(standardIdentifiers);

		return result;
	}

	public static List<ImportPublicationNameDetails> ParseDaWinciPublicationNames(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationNameDetails> result = new();

		var publicationDetailsElements = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										where element.Attribute(DAWINCI_TAG)?.Value == "200"
										select element;

		var publicationNames = ParseDaWinciPublicationNamesFromElements(publicationDetailsElements, xmlns);
		result.AddRange(publicationNames);

		return result;
	}

	public static ImportPublication ParseDaWinciSourcePublication(XElement sourcePublicationElement, string xmlns)
	{
		ImportPublication result = new();

		result.PublicationIds = ParseDaWinciSourcePublicationIdentifiers(sourcePublicationElement, xmlns);
		result.PublicationExternDbIds = ParseDaWinciSourcePublicationExternDbIds(sourcePublicationElement, xmlns);
		result.PublicationNames = ParseDaWinciSourcePublicationNames(sourcePublicationElement, xmlns);

		return result;
	}

	public static List<ImportPublicationNameDetails> ParseDaWinciSourcePublicationNames(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationNameDetails> result = new();

		var publicationDetailsElements =   from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_DATAFIELD, xmlns))
										   where element.Attribute(DAWINCI_TAG)?.Value == "200"
										   select element;

		var publicationNames = ParseDaWinciPublicationNamesFromElements(publicationDetailsElements, xmlns);
		result.AddRange(publicationNames);
		return result;
	}

	public static List<ImportPublicationIdentifier> ParseDaWinciSourcePublicationIdentifiers(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();
		var identifierElements = (from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_DATAFIELD, xmlns))
								  where element.Attribute(DAWINCI_TAG)?.Value == "010" ||
								  element.Attribute(DAWINCI_TAG)?.Value == "011" ||
								  element.Attribute(DAWINCI_TAG)?.Value == "013" ||
								  element.Attribute(DAWINCI_TAG)?.Value == "015" ||
								  element.Attribute(DAWINCI_TAG)?.Value == "016" 
								  select element);

		var standardIdentifiers = ParseDaWinciPublicationStandardIdentifiers(identifierElements, xmlns);
		result.AddRange(standardIdentifiers);

		return result;
	}

	public static List<ImportPublicationExternDbId> ParseDaWinciSourcePublicationExternDbIds(XElement sourcePublicationElement, string xmlns)
	{
		List<ImportPublicationExternDbId> result = new();

		var identifierElement = (from element in sourcePublicationElement.Descendants(XName.Get(DAWINCI_CONTROLFIELD, xmlns))
								 where element.Attribute(DAWINCI_TAG)?.Value == "001"
								 select element).FirstOrDefault();
		if (identifierElement is null)
		{
			return result;
		}

		string input = identifierElement.Value;
		string systemName = ZU_PUBLICATIONID_PREFIX;
		if (input.StartsWith("CREPC"))
		{
			systemName = CREPC_IDENTIFIER_PREFIX;
			input = input.Substring(5);
		}
		ImportPublicationExternDbId newIdentifier = new()
		{
			PublicationId = $"{systemName}:{input}"
		};
		result.Add(newIdentifier);
		return result;
	}


	public static List<ImportPublicationNameDetails> ParseDaWinciPublicationNamesFromElements(
		IEnumerable<XElement> publicationDetailsElement,
		string xmlns)
	{
		List<ImportPublicationNameDetails> result = new();

		if(!publicationDetailsElement.Any())
		{
			return result;
		}
		foreach (var detailElement in publicationDetailsElement)
		{
			var nameElement = (from element in detailElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
							   where element.Attribute(DAWINCI_CODE)?.Value == "a"
							   select element).FirstOrDefault();
			if (nameElement is null)
			{
				continue;
			}

			var name = nameElement.Value;
			var publicationNameDetails = new ImportPublicationNameDetails
			{
				Name = name,
				NameType = "title_proper"
			};
			result.Add(publicationNameDetails);
		}
		return result;
	}

	public static List<ImportPublicationIdentifier> ParseDaWinciPublicationStandardIdentifiers(
		IEnumerable<XElement> records, 
		string xmlns)
	{
		List<ImportPublicationIdentifier> result = new();
		var intStandardTypes = new Dictionary<string, string>
		{
			{"010", "isbn"},
			{"011", "issn"},
			{"013", "ismn"},
			{"015", "isrn"},
			{"016", "isrc"}
		};

		if (records != null)
		{
			foreach (var recordElement in records)
			{
				var tag = recordElement.Attribute(DAWINCI_TAG)?.Value;
				if (tag is null || !intStandardTypes.TryGetValue(tag, out var isName))
				{
					continue;
				}
				var idValue = recordElement.Value.Trim();

				var inputArr = idValue.Split(' ');
				string isForm = null;
				if (inputArr.Length > 1)
				{
					idValue = inputArr[0];
					isForm = inputArr[1];
				}
				var publicationId = PublicationIdCreator(isName, idValue, isForm);
				if (publicationId != null)
				{
					result.Add(publicationId);
				}
			}
		}
		return result;
	}

}
