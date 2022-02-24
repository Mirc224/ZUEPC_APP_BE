using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionData
{
	Task<InstitutionModel?> GetInstitutionByIdAsync(long id);
	Task<IEnumerable<InstitutionModel>> GetAllInstitutionsAsync();
	Task<long> InsertInstitutionAsync(InstitutionModel model);
	Task<int> UpdateInstitutionAsync(InstitutionModel model);
	Task<int> DeleteInstitutionByIdAsync(long id);
}
