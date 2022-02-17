using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCPublicationTitle
{
	[XmlText]
	public string? Title { get; set; }
	[XmlAttribute(AttributeName = "title_type")]
	public string? TitleType { get; set; }
	[XmlAttribute(AttributeName = "lang")]
	public string? Language { get; set; }
}
