using Dapper;
using DataAccess.Data.User;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLUserData : 
	SQLDbRepositoryWithFilterBase<UserModel, UserFilter>, 
	IUserData
{
	public SQLUserData(ISqlDataAccess db)
	: base(db, TableNameConstatnts.USERS_TABLE, TableAliasConstants.USERS_TABLE_ALIAS) 
	{
	}

	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		builder.Where("Email = @Email");
		return (await GetModelsAsync(new { Email = email }, builder)).FirstOrDefault();
	}

	protected override dynamic BuildJoinExpression(SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		builder.InnerJoin($@"{TableNameConstatnts.USER_ROLES_TABLE} as {TableAliasConstants.USER_ROLES_TABLE_ALIAS} ON 
							{baseTableAlias}.Id = {TableAliasConstants.USER_ROLES_TABLE_ALIAS}.UserId");
		return parameters;
	}

	protected override dynamic BuildWhereExpressionAndGetParameters(UserFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Email != null)
		{
			builder.WhereInArray(nameof(queryFilter.Email), queryFilter.Email, baseTableAlias, parameters);
		}
		if (queryFilter.FirstName != null && queryFilter.FirstName.Any())
		{
			builder.WhereInArray(nameof(queryFilter.FirstName), queryFilter.FirstName, baseTableAlias, parameters);
		}
		if (queryFilter.LastName != null)
		{
			builder.WhereInArray(nameof(queryFilter.LastName), queryFilter.LastName, baseTableAlias, parameters);
		}
		if (queryFilter.UserRole != null)
		{
			builder.WhereInArray("RoleId", queryFilter.UserRole, TableAliasConstants.USER_ROLES_TABLE_ALIAS, parameters);
		}
		return parameters;
	}

	protected override dynamic BuildGroupByExpression(SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		builder.GroupBy(baseSelect);
		return parameters;
	}
}
