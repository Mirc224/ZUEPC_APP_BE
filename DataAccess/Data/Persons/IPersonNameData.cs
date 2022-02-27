using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonNameData
{
	Task<PersonNameModel?> GetPersonNameByIdAsync(long id);
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId);
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByFirstNameAsync(string firstName);
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByLastNameAsync(string lastName);
	Task<long> InsertPersonNameAsync(PersonNameModel model);
	Task<int> DeletePersonNameByIdAsync(long id);
	Task<int> DeletePersonNameByPersonIdAsync(long personId);
	Task<int> UpdatePersonNameAsync(PersonNameModel model);
}
