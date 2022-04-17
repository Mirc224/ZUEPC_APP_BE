using ZUEPC.Base.Enums.Publications;
using ZUEPC.Import.Models.Common;

namespace ZUEPC.Import.Models;

public class ImportPublication
{
	public List<ImportPublicationExternDatabaseId> PublicationExternDbIds { get; set; } = new();
	public List<ImportPublicationIdentifier> PublicationIdentifiers { get; set; } = new();
	public PublicationType PublicationType { get; set; }
	public List<ImportPublicationName> PublicationNames { get; set; } = new();
	public List<ImportPublicationAuthor> PublicationAuthors { get; set; } = new();
	public List<ImportRelatedPublication> RelatedPublications { get; set; } = new();
	public List<ImportPublicationActivity> PublicationActivities { get; set; } = new();
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }

	public string? PublicationTypeAsString
	{
		set
		{
			Dictionary<string, PublicationType> publicationTypeMap = new()
			{
				{ "formCasopis_conf.xml", PublicationType.PERIODICAL },
				{ "formClanok_conf.xml", PublicationType.ARTICLE },
				{ "formMonografia_conf.xml", PublicationType.MONOGRAPH },
				{ "formPrispevokZbornik_conf.xml", PublicationType.CONTRIBUTION_PROCEEDINGS },
				{ "formSprava_conf.xml", PublicationType.REPORT },
				{ "formZbornik_conf.xml", PublicationType.PROCEEDINGS },
				{ "formNorma_conf.xml", PublicationType.NORM },
				{ "formPatent_conf.xml", PublicationType.PATENT },
				{ "formBookPublication_conf.xml", PublicationType.BOOK_PUBLICATION }
			};
			if (value != null && publicationTypeMap.TryGetValue(value, out PublicationType publicationType))
			{
				PublicationType = publicationType;
				return;
			}
			PublicationType = default;
		}
	}

	public class ImportPublicationName
	{
		public string? Name { get; set; }
		public string? NameType { get; set; }
	}


	public class ImportPublicationIdentifier
	{
		public string? IdentifierValue { get; set; }
		public string? IdentifierName { get; set; }
		public string? ISForm { get; set; }
	}

	public class ImportPublicationExternDatabaseId : EPCImportExternDatabaseIdBase
	{
	}
}
