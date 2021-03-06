using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionExternDatabaseIdData : 
	IRepositoryBase<InstitutionExternDatabaseIdModel>,
	IRepositoryWithSimpleIdBase<InstitutionExternDatabaseIdModel, long>
{
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByInstitutionIdInSetAsync(IEnumerable<long> institutionIds);
	Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
}
