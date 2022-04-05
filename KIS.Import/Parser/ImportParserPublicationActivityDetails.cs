using System.Xml.Linq;
using ZUEPC.Base.Extensions;
using ZUEPC.Import.Models;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static List<ImportPublicationActivity> ParseCREPCPublishingActivityDetails(
		XElement publicationElement,
		string xmlns)
	{
		List<ImportPublicationActivity> result = new();
		IEnumerable<XElement>? biblioActivityElements = publicationElement.Elements(XName.Get("cross_biblio_activity", xmlns));

		foreach (XElement activityElement in biblioActivityElements.OrEmptyIfNull())
		{
			XElement? recActivityElement = activityElement.Element(XName.Get("rec_activity_crepc", xmlns));
			if (recActivityElement is null)
			{
				continue;
			}

			ImportPublicationActivity activityDetails = new()
			{
				Category = recActivityElement.Element(XName.Get("category", xmlns))?.Value.Trim(),
				GovernmentGrant = recActivityElement.Element(XName.Get("government_grant", xmlns))?.Value.Trim()
			};
			result.Add(activityDetails);
		}

		return result;
	}

	private static List<ImportPublicationActivity> ParseDaWinciPublishingActivityDetails(
		XElement publicationElement,
		string xmlns)
	{
		List<ImportPublicationActivity> result = new();

		IEnumerable<XElement>? publishingActivityElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
															where element.Attribute(DAWINCI_TAG)?.Value == "985"
															select element);

		foreach (XElement activityElement in publishingActivityElement.OrEmptyIfNull())
		{
			XElement? categoryElement = (from element in activityElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
										 where element.Attribute(DAWINCI_CODE)?.Value == "a"
										 select element).FirstOrDefault();
			XElement? governmentGrantElement = (from element in activityElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
												where element.Attribute(DAWINCI_CODE)?.Value == "g"
												select element).FirstOrDefault();
			if (categoryElement is null && governmentGrantElement is null)
			{
				continue;
			}

			XElement? activityYearElement = (from element in activityElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
											 where element.Attribute(DAWINCI_CODE)?.Value == "r"
											 select element).FirstOrDefault();

			ImportPublicationActivity activityDetails = new()
			{
				Category = categoryElement?.Value.Trim(),
				GovernmentGrant = governmentGrantElement?.Value.Trim(),
				ActivityYear = ParseInt(activityYearElement?.Value)
			};
			result.Add(activityDetails);
		}

		return result;
	}
}
