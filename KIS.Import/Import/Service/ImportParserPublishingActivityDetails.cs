using System.Xml.Linq;
using ZUEPC.Import.Import.Models;

namespace ZUEPC.Import.Import.Service;

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
}
