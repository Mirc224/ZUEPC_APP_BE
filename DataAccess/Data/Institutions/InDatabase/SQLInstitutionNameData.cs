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
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		AddToWhereExpression(nameof(InstitutionNameModel.InstitutionId), institutionId, builder, parameters);
		return await DeleteModelAsync(parameters, builder);
	}

	public async Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(InstitutionNameModel.InstitutionId), institutionId, builder, parameters);
		return (await GetModelsAsync(parameters, builder));
	}
}
