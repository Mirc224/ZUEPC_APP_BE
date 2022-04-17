using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public interface IPersonData : 
	IRepositoryBase<PersonModel>,
	IRepositoryWithSimpleIdBase<PersonModel, long>,
	IRepositoryWithFilter<PersonModel, PersonFilter>
{
}
