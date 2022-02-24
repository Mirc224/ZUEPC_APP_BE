using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionExternDatabaseIdData
{
	Task<InstitutionExternDatabaseIdModel?> GetInstitutionExternDatabaseIdByIdAsync(long id);
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByExternIdAsync(string externDbId);
	Task<long> InsertInstitutionExternDatabaseIdAsync(InstitutionExternDatabaseIdModel model);
	Task<int> UpdateInstitutionExternDatabaseIdAsync(InstitutionExternDatabaseIdModel model);
	Task<int> DeleteInstitutionExternDatabaseIdByIdAsync(long id);
	Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
}
