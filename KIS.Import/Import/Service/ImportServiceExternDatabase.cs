using System.Xml.Linq;
using ZUEPC.Import.Import.Models;
using static ZUEPC.Import.Import.Models.ImportExternDatabase;

namespace ZUEPC.Import.Import.Service;

partial class ImportService
{
	public static ImportExternDatabase? CREPCExternDbCreator(XElement extDbElement, string xmlns)
	{
		ImportExternDatabase externDatabase = new();
		externDatabase.CREPCId = extDbElement.Attribute("id")?.Value;
		var dbNamerecords = extDbElement.Elements(XName.Get("name", xmlns));

		foreach (var dbNameElement in dbNamerecords)
		{
			var dbName = new ImportExternDatabaseName();
			dbName.Name = dbNameElement.Value;
			dbName.NameType = dbNameElement.Attribute("name_type")?.Value;
			externDatabase.DatabaseNames.Add(dbName);
		}
		return externDatabase;
	}
}