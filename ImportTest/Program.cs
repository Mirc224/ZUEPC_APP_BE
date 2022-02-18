// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZUEPC.Import.Enums.Person;
using ZUEPC.Import.Enums.Publication;
using ZUEPC.Import.ImportModels.CREPC.Publication;

public class Program
{
	static void Main()
	{
		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\AFC hromadné 13x.xml");
		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\moj_testovaci.xml");

		//XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		//XNamespace opensearch = "http://biblib.net/opensearch/";
		//XNamespace xmlns = "http://www.crepc.sk/schema/xml-crepc2/";
		//XNamespace biblibsearch = "http://biblib.net/search/";
		////var nodes = doc.Descendants(xmlns + "rec_biblio");
		//var searchedName = xmlns + "rec_biblio";

		//var nodes = from descendant in doc.Descendants()
		//			where descendant.Name == searchedName &&
		//			!(from ancestor in descendant.Ancestors()
		//			  where ancestor.Name == searchedName
		//			  select ancestor).Any()
		//			select descendant;


		XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z DaWinci\Testovacie\AFC_hromadne_13x.ISO");
		doc = ConvertUNIMARCToReadableForm(doc);
		Console.WriteLine(doc.ToString());

		XNamespace marc = "http://www.loc.gov/MARC21/slim";
		var searchedName = "record";

		var nodes = from descendant in doc.Descendants()
					where descendant.Name == searchedName &&
					!(from ancestor in descendant.Ancestors()
					  where ancestor.Name == searchedName
					  select ancestor).Any()
					select descendant;

		//Console.WriteLine(nodes.Count());
		foreach (var child in nodes)
		{
			StringReader reader = new StringReader(child.ToString());
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DaWinciPublication));
			var result = xmlSerializer.Deserialize(reader) as DaWinciPublication;
			Console.WriteLine();
		}

		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z DaWinci\Testovacie\ADC_s_ohlasmi.ISO");
		Console.WriteLine(doc.ToString());
	}

	[XmlRoot(ElementName = "record")]
	public class DaWinciPublication
	{
		[XmlElement(ElementName = "publication_identifier")]
		public string? KISId { get; set; }

		[XmlArray(ElementName = "publication_language")]
		[XmlArrayItem(ElementName = "language_code")]
		public string[]? Language { get; set; }

		[XmlElement(ElementName = "publication_details")]
		public DaWinciPublicationDetails? PublicationDetails { get; set; }
	}

	public class DaWinciPublicationDetails
	{
		[XmlElement(ElementName = "name")]
		public string? Title { get; set; }

		[XmlElement(ElementName = "document_form")]
		public string? DocumentForm { get; set; }

	}


	public static void FindElementAndRename(XDocument doc, string rootName, string elementName, string attributeName, string attributeValue, string newElementName)
	{
		var wantedElements = from descendant in doc.Descendants(rootName) select descendant;
		foreach (var wantedElement in wantedElements)
		{
			var records = from descendant in wantedElement.Descendants()
						  where descendant.Name == elementName && descendant?.Attribute(attributeName)?.Value == attributeValue
						  select descendant;
			foreach (var record in records)
			{
				record.Name = newElementName;
			}
		}
	}

	public static XDocument ConvertUNIMARCToReadableForm(XDocument doc)
	{
		XNamespace marc = "http://www.loc.gov/MARC21/slim";
		var controlfieldName = (marc + "controlfield").ToString();
		var subfieldName = (marc + "subfield").ToString();
		var datafieldName = (marc + "datafield").ToString();
		var marcCollection = (marc + "collection").ToString();

		var records = from descendant in doc.Descendants(marc+"record") select descendant;
		foreach (var record in records)
		{
				record.Name = "record";
		}

		FindElementAndRename(doc, marcCollection, controlfieldName, "tag", "001", "publication_identifier");
		FindElementAndRename(doc, marcCollection, controlfieldName, "tag", "005", "version_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "010", "isbn_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "011", "issn_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "013", "ismn_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "015", "isrn_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "016", "isrc_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "100", "general_processing_data");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "101", "publication_language");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "102", "publication_country");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "200", "publication_details");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "210", "publishing_info");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "225", "edition_details");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "301", "related_project");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "463", "source_publication");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "610", "subject_terms");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "700", "publication_person");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "701", "publication_person");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "710", "related_event");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "801", "originating_source");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "856", "access_locations");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "913", "other_identifier");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "974", "publication_processor");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "976", "reference_to_publication");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "985", "publishing_activity");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "992", "document_type");
		FindElementAndRename(doc, marcCollection, datafieldName, "tag", "RID", "extern_db_identifier");

		FindElementAndRename(doc, "publication_language", subfieldName, "code", "a", "language_code");
		FindElementAndRename(doc, "publication_country", subfieldName, "code", "a", "country_code");
		
		FindElementAndRename(doc, "isbn_identifier", subfieldName, "code", "a", "number");
		FindElementAndRename(doc, "issn_identifier", subfieldName, "code", "a", "number");
		FindElementAndRename(doc, "ismn_identifier", subfieldName, "code", "a", "number");
		FindElementAndRename(doc, "isrn_identifier", subfieldName, "code", "a", "number");
		FindElementAndRename(doc, "isrc_identifier", subfieldName, "code", "a", "number");

		FindElementAndRename(doc, "publication_details", subfieldName, "code", "a", "name");
		FindElementAndRename(doc, "publication_details", subfieldName, "code", "b", "document_form");
		FindElementAndRename(doc, "publication_details", subfieldName, "code", "c", "statement_of_responsibility");

		FindElementAndRename(doc, "publishing_info", subfieldName, "code", "a", "publishing_town");
		FindElementAndRename(doc, "publishing_info", subfieldName, "code", "d", "publishing_date");
		FindElementAndRename(doc, "publishing_info", subfieldName, "code", "e", "publishing_country");

		FindElementAndRename(doc, "subject_terms", subfieldName, "code", "a", "term");

		FindElementAndRename(doc, "related_project", subfieldName, "code", "a", "project_name");
		FindElementAndRename(doc, "related_project", subfieldName, "code", "y", "number");
		FindElementAndRename(doc, "related_project", subfieldName, "code", "x", "grant_scheme");

		FindElementAndRename(doc, "publication_person", subfieldName, "code", "a", "first_name");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "b", "last_name");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "f", "birth_death_date");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "p", "institution_tag");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "3", "person_identifier");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "4", "function_code");
		FindElementAndRename(doc, "publication_person", subfieldName, "code", "9", "contribution_ratio");

		FindElementAndRename(doc, "related_event", subfieldName, "code", "a", "event_name");
		FindElementAndRename(doc, "related_event", subfieldName, "code", "d", "number");
		FindElementAndRename(doc, "related_event", subfieldName, "code", "f", "dates_interval");
		FindElementAndRename(doc, "related_event", subfieldName, "code", "e", "town");
		FindElementAndRename(doc, "related_event", subfieldName, "code", "g", "country");

		FindElementAndRename(doc, "originating_source", subfieldName, "code", "a", "country");
		FindElementAndRename(doc, "originating_source", subfieldName, "code", "b", "catalog_agency");
		FindElementAndRename(doc, "originating_source", subfieldName, "code", "c", "transaction_date");
		FindElementAndRename(doc, "originating_source", subfieldName, "code", "g", "catalog_rules");

		FindElementAndRename(doc, "access_locations", subfieldName, "code", "u", "uri");
		FindElementAndRename(doc, "access_locations", subfieldName, "code", "z", "source_uri");

		FindElementAndRename(doc, "reference_to_publication", subfieldName, "code", "d", "year");
		FindElementAndRename(doc, "reference_to_publication", subfieldName, "code", "i", "description");
		FindElementAndRename(doc, "reference_to_publication", subfieldName, "code", "c", "db_name");

		FindElementAndRename(doc, "other_identifier", subfieldName, "code", "a", "doi");

		FindElementAndRename(doc, "publication_processor", subfieldName, "code", "a", "processor_code");

		FindElementAndRename(doc, "document_type", subfieldName, "code", "a", "type_short");

		FindElementAndRename(doc, "extern_db_identifier", subfieldName, "code", "a", "id");
		FindElementAndRename(doc, "extern_db_identifier", subfieldName, "code", "b", "db_name");

		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "a", "document_category_code");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "s", "research_area");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "3", "crepc_id");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "f", "faculty_short");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "k", "department_short");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "r", "year");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "s", "research_area");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "w", "number_of_authors");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "z", "reviewed");
		FindElementAndRename(doc, "publishing_activity", subfieldName, "code", "q", "mention_workplace");

		FindElementAndRename(doc, "extern_db_identifier", subfieldName, "code", "a", "publication_identifier");
		FindElementAndRename(doc, "extern_db_identifier", subfieldName, "code", "b", "database_name");

		records = from descendant in doc.Descendants("source_publication") select descendant;
		foreach(var record in records)
		{
			var insertedElements = new List<XElement>();
			var replacedFields = from replacedRelField in record.Descendants(marc + "relfield") select replacedRelField;
			foreach (var relField in replacedFields)
			{
				insertedElements.AddRange(relField.Elements());
			}
			record.RemoveAll();
			record.Add(insertedElements);
		}

		return doc;
	}
}