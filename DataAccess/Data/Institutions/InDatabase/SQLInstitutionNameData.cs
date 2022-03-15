using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions.InDatabase;

public class SQLInstitutionNameData :
	SQLDbRepositoryBase<InstitutionNameModel>,
	IInstitutionNameData
{
	public SQLInstitutionNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.INSTITUTION_NAMES_TABLE, TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		return await DeleteModelsWithColumnValue(nameof(InstitutionNameModel.InstitutionId), institutionId);
	}

	public async Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		return await GetModelsWithColumnValue(nameof(InstitutionNameModel.InstitutionId), institutionId);
	}
}
