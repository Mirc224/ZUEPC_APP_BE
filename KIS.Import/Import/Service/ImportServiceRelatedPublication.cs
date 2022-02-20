using System.Xml.Linq;
using ZUEPC.Import.Import.Models;

namespace ZUEPC.Import.Import.Service;

partial class ImportService
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
			relatedPublication.RelatedPublication = ParseCREPCPublication(nestedPublicationElement, xmlns);
			result.Add(relatedPublication);
		}
		return result;
	}
}
