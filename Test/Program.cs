// See https://aka.ms/new-console-template for more information
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZUEPC.Import.Enums.Person;
using ZUEPC.Import.Enums.Publication;

public class Program
{
	static void Main()
	{
		XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\moj_testovaci.xml");

		XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		XNamespace opensearch = "http://biblib.net/opensearch/";
		XNamespace xmlns = "http://www.crepc.sk/schema/xml-crepc2/";
		XNamespace biblibsearch = "http://biblib.net/search/";
		//var nodes = doc.Descendants(xmlns + "rec_biblio");
		var searchedName = xmlns + "rec_biblio";

		var nodes = from descendant in doc.Descendants()
					where descendant.Name == searchedName &&
					!(from ancestor in descendant.Ancestors()
					  where ancestor.Name == searchedName
					  select ancestor).Any()
					select descendant;

		Console.WriteLine(nodes.Count());
		foreach (var child in nodes)
		{
			StringReader reader = new StringReader(child.ToString());
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CREPCPublicationImport));
			var result = xmlSerializer.Deserialize(reader) as CREPCPublicationImport;
			Console.WriteLine(result.CREPCId);
		}
	}

	[XmlRoot(ElementName = "rec_biblio", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
	public class CREPCPublicationImport
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

		[XmlAttribute(AttributeName = "form_type")]
		public string? PublicationTypeAsString
		{
			get => _publicationTypeAsString;
			set
			{
				_publicationTypeAsString = value;
				Dictionary<string, PublicationType> _publicationTypeMap = new()
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
				if (value != null && _publicationTypeMap.TryGetValue(value, out var publicationType))
				{
					PublicationType = publicationType;
					return;
				}
				PublicationType = default;
			}
		}

		public string? _publicationTypeAsString;
	}

	public class CREPCPublicationDigitalIdentifier : CREPCPublicationIdentifier
	{
		[XmlAttribute(AttributeName = "di_type")]
		public override string IdentifierTypeString
		{
			set => base.IdentifierTypeString = value;
			get => base.IdentifierTypeString;
		}
		[XmlElement(ElementName = "digi_value")]
		public override string? IdentifierValue { get; set; }
	}

	public class CREPCPublicationInternationalIdentifier : CREPCPublicationIdentifier
	{

		[XmlAttribute(AttributeName = "is_type")]
		public override string IdentifierTypeString
		{
			set => base.IdentifierTypeString = value;
			get => base.IdentifierTypeString;
		}
		[XmlElement(ElementName = "number")]
		public override string? IdentifierValue { get; set; }
		[XmlAttribute(AttributeName = "is_form")]
		public string? IdentifierForm { get; set; }
	}

	public abstract class CREPCPublicationIdentifier
	{
		public virtual PublicationIdentifierType IdentifierType { get; set; }
		[XmlIgnore]
		public virtual string? IdentifierValue { get; set; }
		[XmlIgnore]
		public virtual string IdentifierTypeString
		{
			get => IdentifierType.ToString();
			set
			{
				Dictionary<string, PublicationIdentifierType> _identifierType = new()
				{
					{ "isbn", PublicationIdentifierType.ISBN },
					{ "DOI", PublicationIdentifierType.DOI }
				};
				if (_identifierType.TryGetValue(value, out var publicationType))
				{
					IdentifierType = publicationType;
					return;
				}
				IdentifierType = default;
			}
		}

	}

	public class CREPCPublicationPerson
	{
		public PersonToPublicationRoleType PersonRole { get; set; }
		[XmlAttribute(AttributeName = "ratio")]
		public string? ShareString
		{
			get => Share?.ToString();
			set
			{
				Share = default;
				string currentDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				var convertedVal = value?.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
				if (double.TryParse(convertedVal, out var tmpNumber))
				{
					Share = tmpNumber;
				}
			}
		}

		[XmlAttribute(AttributeName = "role")]
		public string? PersonRoleString
		{
			get => _personRoleString;
			set
			{
				_personRoleString = value;
				Dictionary<string, PersonToPublicationRoleType> _personRoleMapping = new()
				{
					{ "author", PersonToPublicationRoleType.AUTHOR },
					{ "compiler", PersonToPublicationRoleType.COMPILER },
					{ "reviewer", PersonToPublicationRoleType.REVIEWER },
					{ "translator", PersonToPublicationRoleType.TRANSLATOR },
					{ "publisher", PersonToPublicationRoleType.PUBLISHER },
					{ "distributor", PersonToPublicationRoleType.DISTRIBUTOR },
					{ "printer", PersonToPublicationRoleType.PRINTER },
					{ "impression", PersonToPublicationRoleType.IMPRESSION },
					{ "editor_in_chief", PersonToPublicationRoleType.EDITOR_IN_CHIEF },
					{ "executive_editor", PersonToPublicationRoleType.EXECUTIVE_EDITOR },
					{ "editor", PersonToPublicationRoleType.EDITOR },
					{ "chairman_of_the_editorial_board", PersonToPublicationRoleType.CHAIRMAN_OF_THE_EDITORIAL_BOARD },
					{ "editor_compiler", PersonToPublicationRoleType.EDITOR_COMPILER },
					{ "author_correspondence", PersonToPublicationRoleType.AUTHOR_CORRESPONDENCE },
					{ "author_comments", PersonToPublicationRoleType.AUTHOR_COMMENTS },
					{ "author_introduction", PersonToPublicationRoleType.AUTHOR_INTRODUCTION },
					{ "author_afterword", PersonToPublicationRoleType.AUTHOR_AFTERWORD },
					{ "author_interview", PersonToPublicationRoleType.AUTHOR_INTERVIEW },
					{ "author_interviewee", PersonToPublicationRoleType.AUTHOR_INTERVIEWEE },
					{ "author_photographs", PersonToPublicationRoleType.AUTHOR_PHOTOGRAPHS },
					{ "author_graphics", PersonToPublicationRoleType.AUTHOR_GRAPHICS },
					{ "author_music", PersonToPublicationRoleType.AUTHOR_MUSIC },
					{ "author_cartographer", PersonToPublicationRoleType.AUTHOR_CARTOGRAPHER },
					{ "author_programmer", PersonToPublicationRoleType.AUTHOR_PROGRAMMER },
					{ "editor_critical", PersonToPublicationRoleType.EDITOR_CRITICAL }
				};
				if (value != null && _personRoleMapping.TryGetValue(value, out var foundValue))
				{
					PersonRole = foundValue;
					return;
				}
				PersonRole = default;
			}
		}
		[XmlElement(ElementName = "rec_person")]
		public CREPCPerson? Person { get; set; }
		[XmlElement(ElementName = "affiliation")]
		public CREPCReportingInstitution[]? ReportingInstitutions { get; set; }

		public double? Share;

		public string? _personRoleString;
	}

	[XmlRoot(ElementName = "rec_person", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
	public class CREPCPerson
	{
		[XmlAttribute(AttributeName = "id")]
		public int CREPCId { get; set; }
		[XmlElement(ElementName = "firstname")]
		public string? FirstName { get; set; }
		[XmlElement(ElementName = "lastname")]
		public string? LastName { get; set; }
		[XmlElement(ElementName = "middle_name")]
		public string? MiddleName { get; set; }
		[XmlElement(ElementName = "cross_person_database")]
		public CREPCPersonIdInOtherDatabase[]? PersonsIdInOtherDatabases { get; set; }
		[XmlElement(ElementName = "cross_person_institution")]
		public CREPCPersonInstitution[]? PersonsInstitution { get; set; }
	}

	public class CREPCPersonInstitution
	{
		[XmlAttribute(AttributeName = "cross_id")]
		public string? CREPCPersonInstitutionId { get; set; }
		public PersonToInstitutionRelationType? PersonToInstitutionRelationType { get; set; }
		[XmlElement(ElementName = "position")]
		public string? Position { get; set; }
		[XmlElement(ElementName = "work_load")]
		public string? WorkLoadString
		{
			get => WorkLoad.ToString();
			set
			{
				WorkLoad = default;
				string currentDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				var convertedVal = value?.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
				if (double.TryParse(convertedVal, out var tmpNumber))
				{
					WorkLoad = tmpNumber;
				}
			}
		}
		[XmlAttribute(AttributeName = "cc_type")]
		public string? ConnectionTypeString
		{
			get => _connectionTypeString;
			set
			{
				_connectionTypeString = value;
			}
		}


		[XmlArray(ElementName = "periods")]
		[XmlArrayItem(ElementName = "date")]
		public CREPCDatePeriod[]? OccupationDates { get; set; }

		public double? WorkLoad { get; set; }
		public string? _connectionTypeString;
	}


	public class CREPCDatePeriod
	{
		public string? PeriodType => Year.PeriodType ?? Month.PeriodType ?? Day.PeriodType ?? default;
		[XmlElement("year")]
		public CREPCDate Year { get; set; } = new() { Value = 1970 };
		[XmlElement("month")]
		public CREPCDate Month { get; set; } = new() { Value = 1 };
		[XmlElement("day")]
		public CREPCDate Day { get; set; } = new() { Value = 1 };
		public DateTime? Date => new DateTime(Year.Value, Month.Value, Day.Value);
	}

	public class CREPCDate
	{
		[XmlText]
		public int Value { get; set; } = 1;
		[XmlAttribute(AttributeName = "period_type")]
		public string? PeriodType { get; set; }
	}

	public class CREPCReportingInstitution
	{
		[XmlAttribute(AttributeName = "cross_id")]
		public string? CREPCPersonReportingInstitutionRecordId { get; set; }
		[XmlAttribute(AttributeName = "bond_type")]
		public string? BindingType { get; set; }
		[XmlElement(ElementName = "rec_institution")]
		public CREPCInstitution? ReportingInstitution { get; set; }
	}

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
		[XmlElement(ElementName = "institution_name")]
		public CREPCInstitutionName[]? InstitutionNames { get; set; }

		public InstitutionType[]? TypeOfInstitution { get; set; }

		[XmlElement(ElementName = "institution_type")]
		public string[]? InstitutionTypeStings
		{
			get => _institutionTypeStrings;
			set
			{
				Dictionary<string, InstitutionType> _institutionTypeMapping = new()
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
					if (_institutionTypeMapping.TryGetValue(x, out var resultVal))
					{
						return resultVal;
					}
					return default;
				}).ToArray();
			}
		}

		private string[]? _institutionTypeStrings;
	}

	public class CREPCPersonIdInOtherDatabase
	{
		[XmlAttribute(AttributeName = "id_value")]
		public string? Id { get; set; }
		[XmlAttribute(AttributeName = "id_title")]
		public string? IdDescription { get; set; }
	}

	public class CREPCInstitutionName
	{
		[XmlAttribute(AttributeName = "inst_type")]
		public string? NameType { get; set; }
		[XmlText]
		public string? Name { get; set; }
		[XmlAttribute(AttributeName = "lang")]
		public string? Language { get; set; }
	}

	public class CREPCInstitutionRelation
	{
		[XmlAttribute(AttributeName = "bond_type")]
		public string? RelationType { get; set; }
		[XmlAttribute(AttributeName = "rec_institution")]
		public CREPCInstitution? RelatedInstitution { get; set; }
	}

	public class CREPCInstitutionIdInOtherDatabase
	{
		[XmlElement(ElementName = "number")]
		public string? Id { get; set; }
		[XmlElement(ElementName = "num_title")]
		public string? IdDescription { get; set; }
	}


	public class CREPCPublicationTitle
	{
		[XmlText]
		public string? Title { get; set; }
		[XmlAttribute(AttributeName = "title_type")]
		public string? TitleType { get; set; }
		[XmlAttribute(AttributeName = "lang")]
		public string? Language { get; set; }
	}


	public enum InstitutionType
	{
		UNKNOWN = 0,
		COLLEGE = 1,
		SLOVAK_ACADEMY_OF_SCIENCE = 2,
		SCIENTIFIC_RESEARCH_INSTITUTE = 3,
		EVENT_ORGANIZER = 4,
		GRANT_AGENCY = 5,
		DATABASE_PRODUCER = 6,
		AWARDING_INSTITUTION = 7,
		PUBLISHER_WITH_REVIEWED_PROCEEDINGS_EVIDENCE = 8,
		PUBLISHER_HAS_NO_REVIEWING_INFORMATION = 9,
		PUBLISHER_RENOWNED = 9,
		OTHER = 10,
		PUBLISHER = 11,
		INTELLECTUAL_PROPERTY_OFFICE = 12
	}

}