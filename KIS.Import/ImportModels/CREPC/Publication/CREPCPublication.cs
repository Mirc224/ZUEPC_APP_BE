using System.Xml.Serialization;
using ZUEPC.Import.Enums.Publication;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

[XmlRoot(ElementName = "rec_biblio", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
public class CREPCPublication
{
	[XmlAttribute(AttributeName = "id")]
	public int CREPCId { get; set; }
	
	[XmlIgnore]
	public PublicationType PublicationType { get; set; }
	
	[XmlArray(ElementName = "biblio_identifier")]
	[XmlArrayItem(ElementName = "digi_identifier", Type = typeof(CREPCPublicationDigitalIdentifier))]
	[XmlArrayItem(ElementName = "int_standards", Type = typeof(CREPCPublicationInternationalIdentifier))]
	public CREPCPublicationIdentifier[]? PublicationIdentifiers { get; set; }
	
	[XmlElement(ElementName = "title")]
	public CREPCPublicationTitle[]? PublicationTitles { get; set; }
	
	[XmlElement(ElementName = "cross_biblio_person")]
	public CREPCPublicationPerson[]? PublicationPersons { get; set; }
	
	[XmlElement(ElementName = "cross_biblio_biblio")]
	public CREPCRelatedPublication[]? RelatedPublications { get; set; }
	
	[XmlAttribute(AttributeName = "legislation")]
	public string? Legislation { get; set; }

	[XmlAttribute(AttributeName = "version")]
	public string? Version { get; set; }

	[XmlElement(ElementName = "document_type")]
	public CREPCDocumentType? DocumentType { get; set; }
	
	[XmlArray(ElementName = "cross_biblio_activity")]
	[XmlArrayItem(ElementName = "rec_activity_crepc")]
	public CREPCPublishingActivity[]? PublishingActivities { get; set; }

	[XmlElement(ElementName = "cross_biblio_database")]
	public CREPCPublicationInDatabase[]? PublicationInOtherDatabases { get; set; }

	[XmlAttribute(AttributeName = "form_type")]
	public string? PublicationTypeAsString
	{
		get => _publicationTypeAsString;
		set
		{
			_publicationTypeAsString = value;
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

	public string? _publicationTypeAsString;
}