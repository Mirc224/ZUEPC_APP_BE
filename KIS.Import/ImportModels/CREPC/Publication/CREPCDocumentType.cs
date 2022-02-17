using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Publication;

public class CREPCDocumentType
{
	[XmlElement(ElementName = "document_type_combo")]
	public string? DocumentType { get; set; }
}
