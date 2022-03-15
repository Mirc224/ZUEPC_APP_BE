using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonNameData :
	SQLDbRepositoryBase<PersonNameModel>,
	IPersonNameData
{
	public SQLPersonNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PERSON_NAMES_TABLE, TableAliasConstants.PERSON_NAME_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonNameByPersonIdAsync(long personId)
	{
		return await DeleteModelsWithColumnValue(nameof(PersonNameModel.PersonId), personId);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId)
	{
		return await GetModelsWithColumnValue(nameof(PersonNameModel.PersonId), personId);
	}
}
