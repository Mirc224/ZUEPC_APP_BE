using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonNameData : 
	IRepositoryBase<PersonNameModel>,
	IRepositoryWithSimpleIdBase<PersonNameModel, long>,
	IRepositoryWithFilter<PersonNameModel, PersonNameFilter>
{
	Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId);
	Task<int> DeletePersonNameByPersonIdAsync(long personId);
}
