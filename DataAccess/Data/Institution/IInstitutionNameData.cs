using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institution;

public interface IInstitutionNameData
{
	Task<InstitutionNameModel?> GetInstitutionNameByIdAsync(long id);
	Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId);
	Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionNameAsync(string institutionName);
	Task<long> InsertInstitutionNameAsync(InstitutionNameModel model);
	Task<int> UpdateInstitutionNameAsync(InstitutionNameModel model);
	Task<int> DeleteInstitutionNameByIdAsync(long id);
	Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId);
}
