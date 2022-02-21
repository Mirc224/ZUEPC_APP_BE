using System.Xml.Linq;
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

		if(records != null)
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
			PublicationId = "CREPC:" + input,
			ExternDatabase = new ImportExternDatabase()
			{
				CREPCId = "CREPC",
				DatabaseNames = new()
				{
					new ImportExternDatabaseName()
					{
						Name = "CREPC",
						NameType = "short_name"
					}
				}
			}
		};

		result.Add(externDbId);

		var records = publicationElement.Elements(XName.Get("cross_biblio_database", xmlns));

		foreach (var recordElement in records)
		{
			ImportPublicationExternDbId publicationExterDbId = new();
			publicationExterDbId.PublicationId = recordElement.Element(XName.Get("database_id", xmlns)).Value;
			publicationExterDbId.ExternDatabase = CREPCExternDbCreator(recordElement.Element(XName.Get("rec_database", xmlns)), xmlns);
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
}
