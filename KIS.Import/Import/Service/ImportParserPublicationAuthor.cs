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
			var authorPerson = ParseCREPCPerson(author.Element(XName.Get("rec_person", xmlns)), xmlns);
			var publicationAuthor = new ImportPublicationAuthor() { Person = authorPerson };
			publicationAuthor.ContributionRatioString = author.Attribute("ratio")?.Value;
			publicationAuthor.Role = author.Attribute("role")?.Value;

			var reportingRelationElement = author.Element(XName.Get("affiliation", xmlns));
			var reportingInstitutionElement = reportingRelationElement.Element(XName.Get("rec_institution", xmlns));
			publicationAuthor.ReportingInstitution = ParseCREPCInstitution(reportingInstitutionElement, xmlns);
			result.Add(publicationAuthor);
		}

		return result;
	}
}