using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonNameData
{
	Task<PersonNameModel?> GetPersonNameByIdAsync(long id);
	Task<IEnumerable<PersonNameModel>> GetAllPersonNamesByPersonIdAsync(long personId);
	Task<IEnumerable<PersonNameModel>> GetAllPersonNamesByFirstNameAsync(string firstName);
	Task<IEnumerable<PersonNameModel>> GetAllPersonNamesByLastNameAsync(string lastName);
	Task<long> InsertPersonNameAsync(PersonNameModel model);
	Task<int> DeletePersonNameByIdAsync(long id);
	Task<int> DeletePersonNameByPersonIdAsync(long personId);
	Task<int> UpdatePersonNameAsync(PersonNameModel model);
}
