using System.Xml.Linq;
using ZUEPC.Import.Models;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	public static List<ImportPublishingActivityDetails> ParseCREPCPublishingActivityDetails(
		XElement publicationElement, 
		string xmlns)
	{
		List<ImportPublishingActivityDetails> result = new();
		var biblioActivityElements = publicationElement.Elements(XName.Get("cross_biblio_activity", xmlns));
		if(!biblioActivityElements.Any())
		{
			return result;
		}
		
		foreach(var activityElement in biblioActivityElements)
		{
			var recActivityElement = activityElement.Element(XName.Get("rec_activity_crepc", xmlns));
			if(recActivityElement is null)
			{
				continue;
			}

			ImportPublishingActivityDetails activityDetails = new()
			{
				Category = recActivityElement.Element(XName.Get("category", xmlns))?.Value,
				GovernmentGrant = recActivityElement.Element(XName.Get("government_grant", xmlns))?.Value
			};
			result.Add(activityDetails);
		}

		return result;
	}

	public static List<ImportPublishingActivityDetails> ParseDaWinciPublishingActivityDetails(
		XElement publicationElement,
		string xmlns)
	{
		List<ImportPublishingActivityDetails> result = new();

		var publishingActivityElement = (from element in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
										 where element.Attribute(DAWINCI_TAG)?.Value == "985"
										 select element);

		foreach (var activityElement in publishingActivityElement)
		{
			var categoryElement = (from element in activityElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									  where element.Attribute(DAWINCI_CODE)?.Value == "a"
									  select element).FirstOrDefault();
			var governmentGrantElement = (from element in activityElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									  where element.Attribute(DAWINCI_CODE)?.Value == "g"
									  select element).FirstOrDefault();
			if (categoryElement is null && governmentGrantElement is null )
			{
				continue;
			}

			ImportPublishingActivityDetails activityDetails = new()
			{
				Category = categoryElement?.Value,
				GovernmentGrant = governmentGrantElement?.Value
			};
			result.Add(activityDetails);
		}

		return result;
	}
}
