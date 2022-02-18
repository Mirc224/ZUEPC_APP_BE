using System.Xml.Serialization;
using ZUEPC.Import.ImportModels.CREPC.Database;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCPublicationInDatabase
{
	[XmlElement(ElementName = "database_id")]
	public string? PublicationId { get; set; }
	[XmlElement(ElementName = "rec_database")]
	public CREPCDatabase? ExternDatabase { get; set; }


}
