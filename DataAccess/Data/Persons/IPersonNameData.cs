using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonNameData : IRepositoryBase<PersonNameModel>
{
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId);
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByFirstNameAsync(string firstName);
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByLastNameAsync(string lastName);
	Task<int> DeletePersonNameByPersonIdAsync(long personId);
}
