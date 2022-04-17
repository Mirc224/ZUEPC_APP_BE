using System.Xml.Linq;
using ZUEPC.Base.Extensions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportExternDatabase;

namespace ZUEPC.Import.Parser;

partial class ImportParser
{
	private static ImportExternDatabase CREPCExternDbCreator(XElement extDbElement, string xmlns)
	{
		ImportExternDatabase externDatabase = new();
		externDatabase.CREPCId = extDbElement.Attribute("id")?.Value;
		IEnumerable<XElement>? dbNamerecords = extDbElement.Elements(XName.Get("name", xmlns));

		foreach (XElement dbNameElement in dbNamerecords.OrEmptyIfNull())
		{
			ImportExternDatabaseName dbName = new();
			dbName.Name = dbNameElement.Value;
			dbName.NameType = dbNameElement.Attribute("name_type")?.Value.Trim();
			externDatabase.DatabaseNames.Add(dbName);
		}
		return externDatabase;
	}
}