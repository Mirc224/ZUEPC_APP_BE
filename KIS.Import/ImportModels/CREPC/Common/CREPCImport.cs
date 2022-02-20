using System.Xml.Serialization;

namespace ZUEPC.Import.ImportModels.CREPC.Common;

[XmlRoot(ElementName = "records", Namespace = "http://biblib.net/search/")]
public class CREPCImport
{
	[XmlElement(ElementName = "record")]
	public CREPCBiblibRecord[]? CREPCRecords { get; set; }
}
