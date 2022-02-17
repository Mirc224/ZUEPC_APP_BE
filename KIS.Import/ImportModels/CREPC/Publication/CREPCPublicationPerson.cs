using System.Xml.Serialization;
using ZUEPC.Import.Enums.Person;
using ZUEPC.Import.ImportModels.CREPC.Institution;
using ZUEPC.Import.ImportModels.CREPC.Person;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

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
			Dictionary<string, PersonToPublicationRoleType> personRoleMapping = new()
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
			if (value != null && personRoleMapping.TryGetValue(value, out var foundValue))
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