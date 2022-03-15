using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonData : 
	IRepositoryBase<PersonModel>,
	IRepositoryWithFilter<PersonModel, PersonFilter>
{
}
