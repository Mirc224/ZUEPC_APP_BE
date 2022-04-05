using System.Text.RegularExpressions;
using System.Xml.Linq;
using ZUEPC.Base.Extensions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static List<ImportRelatedPublication> ParseCREPCRelatedPublications(XElement publicationElement, string xmlns)
	{
		List<ImportRelatedPublication> result = new();

		IEnumerable<XElement> relatedPublicationsElements = from node in publicationElement.Elements(XName.Get("cross_biblio_biblio", xmlns))
															 select node;

		foreach (XElement relatedPublicationElement in relatedPublicationsElements.OrEmptyIfNull())
		{
			ImportRelatedPublication relatedPublication = new()
			{
				RelationType = relatedPublicationElement.Attribute("source")?.Value.Trim() ??
				relatedPublicationElement.Attribute("bond_type")?.Value.Trim()
			};

			XElement? nestedPublicationElement = relatedPublicationElement.Element(XName.Get("rec_biblio", xmlns));
			if (nestedPublicationElement is null)
			{
				continue;
			}

			relatedPublication.RelatedPublication = ParseCREPCPublication(nestedPublicationElement, xmlns);
			relatedPublication.CitationCategory = relatedPublicationElement.Element(XName.Get("citation_category", xmlns))?.Value.Trim();
			result.Add(relatedPublication);
		}
		return result;
	}

	private static List<ImportRelatedPublication> ParseDaWinciRelatedPublications(XElement publicationElement, string xmlns)
	{
		List<ImportRelatedPublication> result = new();

		XElement? sourceElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
								   where element.Attribute(DAWINCI_TAG)?.Value == "463"
								   select element).FirstOrDefault();
		if (sourceElement != null)
		{
			ImportRelatedPublication newRelatedPublication = new();
			newRelatedPublication.RelatedPublication = ParseDaWinciSourcePublication(sourceElement, xmlns);
			newRelatedPublication.RelationType = "source";
			result.Add(newRelatedPublication);
		}

		IEnumerable<XElement> relatedPublicationsElements = from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
															 where element.Attribute(DAWINCI_TAG)?.Value == "976"
															 select element;

		foreach (XElement relatedPublicationElement in relatedPublicationsElements.OrEmptyIfNull())
		{
			ImportPublication? parsedPublication = ParseDaWinciReferencePublication(relatedPublicationElement, xmlns);
			if (parsedPublication is null)
			{
				continue;
			}

			string? citationCategory = null;
			XElement? citationCategoryElement = (from element in relatedPublicationElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
												 where element.Attribute(DAWINCI_CODE)?.Value == "4"
												 select element).FirstOrDefault();
			if (citationCategoryElement != null)
			{
				citationCategory = citationCategoryElement.Value.Trim();
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

	private static ImportPublication? ParseDaWinciReferencePublication(XElement referencedPublication, string xmlns)
	{

		string? publicationDetailsString = (from element in referencedPublication.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
											where element.Attribute(DAWINCI_CODE)?.Value == "i"
											select element.Value).FirstOrDefault();
		if (publicationDetailsString is null)
		{
			return default;
		}
		List<ImportPublicationName> publicationNames = new() { new() { NameType = "title_proper", Name = publicationDetailsString } };
		ImportPublication result = new()
		{
			PublicationNames = publicationNames
		};

		string[] wantedIdentifiers = new[] { "issn", "isbn", "ismn", "isrn", "isrc" };
		string sourceDelimeter = ". In: ";
		int startIndexOfPubMetaData = publicationDetailsString.LastIndexOf(sourceDelimeter);
		ImportPublication? sourcePublication = null;
		string? sourceMetadataString = null;
		
		if (startIndexOfPubMetaData != -1)
		{
			sourceMetadataString = publicationDetailsString[(startIndexOfPubMetaData + sourceDelimeter.Length)..];
			sourcePublication = ParseDaWinciReferencePublicationSource(sourceMetadataString, wantedIdentifiers);
		}
		string publicationDetailSubstring = publicationDetailsString[startIndexOfPubMetaData..];

		string? publicationName = publicationDetailsString[..startIndexOfPubMetaData];
		publicationNames[0].Name = publicationName.Trim();
		List<ImportPublicationIdentifier> publicationIdentifiers = new();
		
		if (sourcePublication != null && sourceMetadataString != null)
		{
			Tuple<int, string>? searchResult = GetFoundIdentifierWithStartIndex(sourceMetadataString, wantedIdentifiers);
			if (searchResult == null)
			{
				return result;
			}

			string externIdentifier = Regex.Replace(sourceMetadataString.Substring(searchResult.Item1), @"\s", "");
			ImportPublicationExternDatabaseId externId = new() { ExternIdentifierValue = externIdentifier };
			result.PublicationExternDbIds.Add(externId);
			return result;
		}

		foreach (string? wantedIdentifier in wantedIdentifiers)
		{
			ImportPublicationIdentifier? identifier = ParseDaWinciSearchForIdentifiersInPublicationDetailsString(publicationDetailsString, wantedIdentifier);
			if (identifier != null)
			{
				publicationIdentifiers.Add(identifier);
			}
		}
		result.PublicationIdentifiers = publicationIdentifiers;
		return result;
	}
	
	private static ImportPublication? ParseDaWinciReferencePublicationSource(string sourceMetadataString, IEnumerable<string> wantedIdentifiers)
	{
		Tuple<int, string>? searchResult = GetFoundIdentifierWithStartIndex(sourceMetadataString, wantedIdentifiers);
		if (searchResult is null)
		{
			return default;
		}

		int startIndexOfIdentifier = searchResult.Item1;
		string foundIdentifier = searchResult.Item2;

		int indexOfNextComma = sourceMetadataString.IndexOf(',', startIndexOfIdentifier);
		string? idValue = sourceMetadataString[(startIndexOfIdentifier + foundIdentifier.Length)..indexOfNextComma].Trim();
		ImportPublication resultPublication = new();
		ImportPublicationIdentifier? identifier = PublicationIdCreator(foundIdentifier, idValue);
		if (identifier != null)
		{
			resultPublication.PublicationIdentifiers.Add(identifier);
		}
		string publicationNameValue = sourceMetadataString[..(startIndexOfIdentifier)].Trim();
		ImportPublicationName publicationName = new() { NameType = "title_proper", Name = publicationNameValue };
		resultPublication.PublicationNames.Add(publicationName);
		return resultPublication;
	}

	private static ImportPublicationIdentifier? ParseDaWinciSearchForIdentifiersInPublicationDetailsString(string publicationDetailsString, string wantedIdentifier)
	{
		int startIndexOfIdentifier = publicationDetailsString.LastIndexOf(wantedIdentifier, StringComparison.OrdinalIgnoreCase);
		if (startIndexOfIdentifier == -1)
		{
			return default;
		}
		int indexOfNextComma = publicationDetailsString.IndexOf(',', startIndexOfIdentifier);
		string? idValue = publicationDetailsString[(startIndexOfIdentifier + wantedIdentifier.Length)..indexOfNextComma].Trim();
		return PublicationIdCreator(wantedIdentifier, idValue);
	}

	private static Tuple<int, string>? GetFoundIdentifierWithStartIndex(string sourceMetadataString, IEnumerable<string> wantedIdentifiers)
	{
		int startIndexOfIdentifier = -1;
		string? foundIdentifier = null;
		foreach (string identifierType in wantedIdentifiers.OrEmptyIfNull())
		{
			startIndexOfIdentifier = sourceMetadataString.LastIndexOf(identifierType, StringComparison.OrdinalIgnoreCase);
			if (startIndexOfIdentifier != -1)
			{
				foundIdentifier = identifierType;
				break;
			}
		}
		if (foundIdentifier is null)
		{
			return default;
		}
		return new Tuple<int, string>(startIndexOfIdentifier, foundIdentifier);
	}

}
