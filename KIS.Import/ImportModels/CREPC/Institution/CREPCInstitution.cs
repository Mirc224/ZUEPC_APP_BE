using System.Xml.Serialization;
using ZUEPC.Import.Enums.Institution;

namespace ZUEPC.Import.ImportModels.CREPC.Institution;

[XmlRoot(ElementName = "rec_institution", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
public class CREPCInstitution
{
	[XmlAttribute(AttributeName = "id")]
	public int CREPCId { get; set; }
	[XmlAttribute(AttributeName = "level")]
	public int InstitutionLevel { get; set; }
	[XmlArray(ElementName = "institution_identifier")]
	[XmlArrayItem(ElementName = "local_numbers")]
	public CREPCInstitutionIdInOtherDatabase[]? InstitutionIdInOtherDatabases { get; set; }
	[XmlElement(ElementName = "institution_tag")]
	public string? InstitutionTag { get; set; }
	[XmlElement(ElementName = "institution_name")]
	public CREPCInstitutionName[]? InstitutionNames { get; set; }

	public InstitutionType[]? TypeOfInstitution { get; set; }

	[XmlElement(ElementName = "institution_type")]
	public string[]? InstitutionTypeStings
	{
		get => _institutionTypeStrings;
		set
		{
			Dictionary<string, InstitutionType> institutionTypeMapping = new()
			{
				{ "college", InstitutionType.COLLEGE },
				{ "Slovak_academy_of_science", InstitutionType.SLOVAK_ACADEMY_OF_SCIENCE },
				{ "scientific_research_institute", InstitutionType.SCIENTIFIC_RESEARCH_INSTITUTE },
				{ "event_organizer", InstitutionType.EVENT_ORGANIZER },
				{ "grant_agency", InstitutionType.GRANT_AGENCY },
				{ "database_producer", InstitutionType.DATABASE_PRODUCER },
				{ "awarding_institution", InstitutionType.AWARDING_INSTITUTION },
				{ "publisher_with_reviewed_proceedings_evidence", InstitutionType.PUBLISHER_WITH_REVIEWED_PROCEEDINGS_EVIDENCE },
				{ "publisher_has_no_reviewing_information", InstitutionType.PUBLISHER_HAS_NO_REVIEWING_INFORMATION },
				{ "publisher___renowned", InstitutionType.PUBLISHER_RENOWNED },
				{ "other", InstitutionType.OTHER },
				{ "publisher", InstitutionType.PUBLISHER },
				{ "intellectual_property_office", InstitutionType.INTELLECTUAL_PROPERTY_OFFICE }
			};

			_institutionTypeStrings = value;

			TypeOfInstitution = value?.Select(x =>
			{
				if (institutionTypeMapping.TryGetValue(x, out var resultVal))
				{
					return resultVal;
				}
				return default;
			}).ToArray();
		}
	}

	private string[]? _institutionTypeStrings;
}