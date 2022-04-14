using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonExternDatabaseIdData : 
	IRepositoryBase<PersonExternDatabaseIdModel>,
	IRepositoryWithSimpleIdBase<PersonExternDatabaseIdModel, long>
{
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId);
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByPersonIdInSetAsync(IEnumerable<long> personIds);
	Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId);
}
