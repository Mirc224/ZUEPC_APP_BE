using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonExternDatabaseIdData
{
	Task<PersonExternDatabaseIdModel?> GetPersonExternDatabaseIdByIdAsync(long id);
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByExternDbIdAsync(string externDbId);
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId);
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<long> InsertPersonExternDatabaseIdAsync(PersonExternDatabaseIdModel model);
	Task<int> UpdatePersonExternDatabaseIdAsync(PersonExternDatabaseIdModel model);
	Task<int> DeletePersonExternDatabaseIdByIdAsync(long id);
	Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId);
}
