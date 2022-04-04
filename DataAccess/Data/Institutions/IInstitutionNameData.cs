using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionNameData : 
	IRepositoryBase<InstitutionNameModel>,
	IRepositoryWithSimpleIdBase<InstitutionNameModel, long>,
	IRepositoryWithFilter<InstitutionNameModel, InstitutionNameFilter>
{
	Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId);
	Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId);
}
