using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionNameData : IRepositoryBase<InstitutionNameModel>
{
	Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId);
	Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionNameAsync(string institutionName);
	Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId);
}
