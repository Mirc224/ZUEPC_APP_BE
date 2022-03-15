using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionExternDatabaseIdData : IRepositoryBase<InstitutionExternDatabaseIdModel>
{
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
	Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId);
}
