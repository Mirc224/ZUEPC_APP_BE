using ZUEPC.Base.Enums.Publication;

namespace ZUEPC.Import.Models;

public class ImportPublication
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public List<ImportPublicationExternDbId> PublicationExternDbIds { get; set; } = new();
	public List<ImportPublicationIdentifier> PublicationIdentifiers { get; set; } = new();
	public PublicationType PublicationType { get; set; }
	public List<ImportPublicationNameDetails> PublicationNames { get; set; } = new();
	public List<ImportPublicationAuthor> PublicationAuthors { get; set; } = new();
	public List<ImportRelatedPublication> RelatedPublications { get; set; } = new();
	public List<ImportPublicationActivityDetails> PublishingActivities { get; set; } = new();
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
			if (value != null && publicationTypeMap.TryGetValue(value, out var publicationType))
			{
				PublicationType = publicationType;
				return;
			}
			PublicationType = default;
		}
	}

	public class ImportPublicationNameDetails
	{
		public string Name { get; set; }
		public string? NameType { get; set; }
	}


	public class ImportPublicationIdentifier
	{
		public string IdentifierValue { get; set; }
		public string IdentifierName { get; set; }
		public string? ISForm { get; set; }
	}

	public class ImportPublicationExternDbId
	{
		public string? PublicationExternIdValue { get; set; }
		//public ImportExternDatabase? ExternDatabase { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
