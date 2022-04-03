using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions.InDatabase;

public class SQLInstitutionExternDatabaseIdData :
	SQLDbEPCRepositoryBase<InstitutionExternDatabaseIdModel>,
	IInstitutionExternDatabaseIdData
{
	public SQLInstitutionExternDatabaseIdData(ISqlDataAccess db) : 
		base(db, TableNameConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(InstitutionExternDatabaseIdModel.InstitutionId), institutionId);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return await GetModelsWithColumnValueInSetAsync(nameof(InstitutionExternDatabaseIdModel.ExternIdentifierValue), identifierValues);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		return await GetModelsWithColumnValueAsync(nameof(InstitutionExternDatabaseIdModel.InstitutionId), institutionId);
	}
}
