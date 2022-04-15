using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonExternDatabaseIdData :
	SQLDbEPCRepositoryBase<PersonExternDatabaseIdModel>,
	IPersonExternDatabaseIdData
{
	public SQLPersonExternDatabaseIdData(ISqlDataAccess db) :
		base(db, TableNameConstants.PERSON_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PersonExternDatabaseIdModel.PersonId), personId);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		if (!identifierValues.Any())
		{
			return Enumerable.Empty<PersonExternDatabaseIdModel>();
		}
		return await GetModelsWithColumnValueInSetAsync(nameof(PersonExternDatabaseIdModel.ExternIdentifierValue), identifierValues);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByPersonIdInSetAsync(IEnumerable<long> personIds)
	{
		if (!personIds.Any())
		{
			return Enumerable.Empty<PersonExternDatabaseIdModel>();
		}
		return await GetModelsWithColumnValueInSetAsync(nameof(PersonNameModel.PersonId), personIds);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PersonExternDatabaseIdModel.PersonId), personId);
	}
}
