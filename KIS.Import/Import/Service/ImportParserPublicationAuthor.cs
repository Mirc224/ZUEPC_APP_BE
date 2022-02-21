using System.Xml.Linq;
using ZUEPC.Import.Import.Models;

namespace ZUEPC.Import.Import.Service;

partial class ImportParser
{
	public static List<ImportPublicationAuthor> ParseCREPCPublicationAuthors(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationAuthor> result = new();

		var publicationAuthors = from node in publicationElement.Elements(XName.Get("cross_biblio_person", xmlns))
								 where (node.Attribute("role")?.Value == "author" || node.Attribute("role")?.Value == "author_correspondence")
								 select node;
		foreach (var author in publicationAuthors)
		{
			var authorElement = author.Element(XName.Get("rec_person", xmlns));
			if(authorElement is null)
			{
				continue;
			}
			var authorPerson = ParseCREPCPerson(authorElement, xmlns);
			var publicationAuthor = new ImportPublicationAuthor() { Person = authorPerson };
			publicationAuthor.ContributionRatioString = author.Attribute("ratio")?.Value;
			publicationAuthor.Role = author.Attribute("role")?.Value;

			var reportingRelationElement = author.Element(XName.Get("affiliation", xmlns));
			if(reportingRelationElement is null)
			{
				result.Add(publicationAuthor);
				continue;
			}
			var reportingInstitutionElement = reportingRelationElement.Element(XName.Get("rec_institution", xmlns));
			if (reportingInstitutionElement is null)
			{
				result.Add(publicationAuthor);
				continue;
			}
			publicationAuthor.ReportingInstitution = ParseCREPCInstitution(reportingInstitutionElement, xmlns);
			result.Add(publicationAuthor);
		}

		return result;
	}

	public static List<ImportPublicationAuthor> ParseDaWinciPublicationAuthors(XElement publicationElement, string xmlns)
	{
		List<ImportPublicationAuthor> result = new();

		var publicationAuthors = from node in publicationElement.Elements(XName.Get(DAWINCI_DATAFIELD, xmlns))
								 where (node.Attribute(DAWINCI_TAG)?.Value == "701" || node.Attribute(DAWINCI_TAG)?.Value == "700") &&
								 (from roleElement in node.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
								 where roleElement.Attribute(DAWINCI_CODE)?.Value == "4"
								 select roleElement.Value).FirstOrDefault() == "070"
								 select node;

		
		foreach (var authorElement in publicationAuthors)
		{
			var authorPerson = ParseDaWinciPerson(authorElement, xmlns);
			var publicationAuthor = new ImportPublicationAuthor() { Person = authorPerson };

			var contributionElement = (from element in authorElement.Elements(XName.Get(DAWINCI_SUBFIELD, xmlns))
									  where element.Attribute(DAWINCI_CODE)?.Value == "9"
									  select element).FirstOrDefault();
			if (contributionElement != null)
			{
				publicationAuthor.ContributionRatioString = contributionElement.Value;
			}
			publicationAuthor.Role = "author";

			publicationAuthor.ReportingInstitution = ParseDaWinciInstitution(authorElement, xmlns);
			result.Add(publicationAuthor);
		}

		return result;
	}
}