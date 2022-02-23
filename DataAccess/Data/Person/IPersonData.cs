using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Person;

public interface IPersonData
{
	Task<PersonModel?> GetPersonByIdAsync(long id);
	Task<IEnumerable<PersonModel>> GetAllPersonsAsync();
	Task<long> InsertPersonAsync(PersonModel model);
	Task<int> DeletePersonByIdAsync(long id);
	Task<int> UpdatePersonAsync(PersonModel model);
}
