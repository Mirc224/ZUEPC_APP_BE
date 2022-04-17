using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionData : 
	IRepositoryBase<InstitutionModel>,
	IRepositoryWithSimpleIdBase<InstitutionModel, long>,
	IRepositoryWithFilter<InstitutionModel, InstitutionFilter>

{
}
