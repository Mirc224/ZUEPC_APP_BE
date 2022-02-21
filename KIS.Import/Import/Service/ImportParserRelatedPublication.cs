using System.Xml.Linq;
using ZUEPC.Import.Import.Models;
using static ZUEPC.Import.Import.Service.ImportPublication;

namespace ZUEPC.Import.Import.Service;

partial class ImportParser
{
	public static List<ImportRelatedPublication> ParseCREPCRelatedPublications(XElement publicationElement, string xmlns)
	{
		List<ImportRelatedPublication> result = new();

		var relatedPublicationsElements = from node in publicationElement.Elements(XName.Get("cross_biblio_biblio", xmlns))
										  select node;

		foreach (var relatedPublicationElement in relatedPublicationsElements)
		{
			ImportRelatedPublication relatedPublication = new()
			{
				RelationType = relatedPublicationElement.Attribute("source")?.Value ??
				relatedPublicationElement.Attribute("bond_type")?.Value
			};

			var nestedPublicationElement = relatedPublicationElement.Element(XName.Get("rec_biblio", xmlns));
			if (nestedPublicationElement is null)
			{
				continue;
			}

			relatedPublication.RelatedPublication = ParseCREPCPublication(nestedPublicationElement, xmlns);
			relatedPublication.CitationCategory = relatedPublicationElement.Element(XName.Get("citation_category", xmlns))?.Value;
			result.Add(relatedPublication);
		}
		return result;
	}

	public static List<ImportRelatedPublication> ParseDaWinciRelatedPublications(XElement publicationElement, string xmlns)
	{
		List<ImportRelatedPublication> result = new();

		var sourceElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
							 where element.Attribute(DAWINCI_TAG)?.Value == "463"
							 select element).FirstOrDefault();
		if (sourceElement != null)
		{
			ImportRelatedPublication newRelatedPublication = new();
			newRelatedPublication.RelatedPublication = ParseDaWinciSourcePublication(sourceElement, xmlns);
			newRelatedPublication.RelationType = "source";
			result.Add(newRelatedPublication);
		}

		var relatedPublicationsElements = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										  where element.Attribute(DAWINCI_TAG)?.Value == "976"
										  select element;

		foreach (var relatedPublicationElement in relatedPublicationsElements)
		{
			var parsedPublication = ParseDaWinciReferencePublication(relatedPublicationElement, xmlns);
			if (parsedPublication is null)
			{
				continue;
			}

			string? citationCategory = null;
			var citationCategoryElement = (from element in relatedPublicationElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										   where element.Attribute(DAWINCI_CODE)?.Value == "4"
										   select element).FirstOrDefault();
			if (citationCategoryElement != null)
			{
				citationCategory = citationCategoryElement.Value;
			}

			ImportRelatedPublication relatedPublication = new()
			{
				RelationType = "response_in",
				RelatedPublication = parsedPublication,
				CitationCategory = citationCategory
			};

			result.Add(relatedPublication);
		}

		return result;
	}

	public static ImportPublication? ParseDaWinciReferencePublication(XElement referencedPublication, string xmlns)
	{

		string? publicationDetailsString = (from element in referencedPublication.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
											where element.Attribute(DAWINCI_CODE)?.Value == "i"
											select element.Value).FirstOrDefault();
		if (publicationDetailsString is null)
		{
			return default;
		}
		var publicationNames = new List<ImportPublicationNameDetails>() { new() { NameType = "title_proper", Name = publicationDetailsString } };
		ImportPublication result = new()
		{
			PublicationNames = publicationNames
		};

		int startIndexOfPubMetaData = publicationDetailsString.LastIndexOf("In: ");
		if (startIndexOfPubMetaData == -1)
		{
			return result;
		}
		string publicationDetailSubstring = publicationDetailsString[startIndexOfPubMetaData..];

		string? publicationName = publicationDetailsString[..startIndexOfPubMetaData];
		publicationNames[0].Name = publicationName;
		List<ImportPublicationIdentifier> publicationIdentifiers = new();

		var wantedIdentifiers = new string[] { "issn", "isbn",  "ismn", "isrn", "isrc"};
		
		foreach(var wantedIdentifier in wantedIdentifiers)
		{
			var identifier = ParseDaWinciSearchForIdentifiersInPublicationDetailsString(publicationDetailsString, wantedIdentifier);
			if(identifier != null)
			{
				publicationIdentifiers.Add(identifier);
			}
		}
		result.PublicationIds = publicationIdentifiers;
		return result;
	}

	public static ImportPublicationIdentifier? ParseDaWinciSearchForIdentifiersInPublicationDetailsString(string publicationDetailsString, string wantedIdentifier)
	{
		int startIndexOfIdentifier = publicationDetailsString.LastIndexOf(wantedIdentifier, StringComparison.OrdinalIgnoreCase);
		if (startIndexOfIdentifier == -1)
		{
			return default;
		}
		int indexOfNextComma = publicationDetailsString.IndexOf(',', startIndexOfIdentifier);
		var idValue = publicationDetailsString[(startIndexOfIdentifier + wantedIdentifier.Length)..indexOfNextComma].Trim();
		return PublicationIdCreator(wantedIdentifier, idValue);
	}

}
