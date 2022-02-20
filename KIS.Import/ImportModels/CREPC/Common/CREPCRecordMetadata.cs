using System.Xml.Serialization;
using ZUEPC.Import.ImportModels.CREPC.Publication;

namespace ZUEPC.Import.ImportModels.CREPC.Common;

public class CREPCRecordMetadata
{
	[XmlElement(ElementName = "rec_biblio", Namespace = "http://www.crepc.sk/schema/xml-crepc2/")]
	public CREPCPublication? Publication { get; set; }
}
