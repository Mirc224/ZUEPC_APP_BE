using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonExternDatabaseIdData :
	SQLDbRepositoryBase<PersonExternDatabaseIdModel>,
	IPersonExternDatabaseIdData
{
	public SQLPersonExternDatabaseIdData(ISqlDataAccess db) :
		base(db, TableNameConstants.PERSON_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return await DeleteModelsWithColumnValue(nameof(PersonExternDatabaseIdModel.PersonId), personId);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return await GetModelsWithColumnValueInSet(nameof(PersonExternDatabaseIdModel.ExternIdentifierValue), identifierValues);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return await GetModelsWithColumnValue(nameof(PersonExternDatabaseIdModel.PersonId), personId);
	}
}
