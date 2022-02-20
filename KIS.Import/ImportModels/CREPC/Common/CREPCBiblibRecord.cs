using System.Xml.Serialization;
using ZUEPC.Import.ImportModels.CREPC.Publication;

namespace ZUEPC.Import.ImportModels.CREPC.Common;

public class CREPCBiblibRecord
{
	[XmlElement(ElementName = "updated")]
	public DateTime RecordVersionDate { get; set; }

	[XmlElement(ElementName = "metadata", Namespace = "http://biblib.net/search/")]
	public CREPCRecordMetadata? Metadata { get; set; }
}
