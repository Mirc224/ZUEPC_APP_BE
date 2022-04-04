using Dapper;
using DataAccess.Data.User;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLUserData : 
	SQLDbRepositoryWithFilterBase<IUserData, UserModel, UserFilter>, 
	IUserData
{
	public SQLUserData(ISqlDataAccess db)
	: base(db, TableNameConstants.USERS_TABLE, TableAliasConstants.USERS_TABLE_ALIAS) 
	{
	}

	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		return (await GetModelsWithColumnValueAsync(nameof(UserModel.Email), email)).FirstOrDefault();
	}

	protected override dynamic BuildJoinWithFilterExpression(UserFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if(queryFilter.UserRole != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(UserModel.Id),
				TableNameConstants.USER_ROLES_TABLE,
				TableAliasConstants.USER_ROLES_TABLE_ALIAS,
				nameof(UserRoleModel.UserId));
		}
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(UserFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Email != null)
		{
			builder.WhereLikeInArray(nameof(UserModel.Email), queryFilter.Email, baseTableAlias, parameters);
		}
		if (queryFilter.Name != null)
		{
			string concatString = builder.GetConcatFunctionString(nameof(UserModel.FirstName), nameof(UserModel.LastName), baseTableAlias, parameters);
			string concatStringReverse = builder.GetConcatFunctionString(nameof(UserModel.LastName), nameof(UserModel.FirstName), baseTableAlias, parameters);
			
			string bindedSql = builder.WhereLikeInArrayBindedString(concatString, queryFilter.Name, "", parameters);
			string bindedSqlReverse = builder.WhereLikeInArrayBindedString(concatStringReverse, queryFilter.Name, "", parameters);

			builder.Where($"({bindedSql} OR {bindedSqlReverse})");
		}
		if (queryFilter.UserRole != null)
		{
			builder.WhereInArray(nameof(UserRoleModel.RoleId), queryFilter.UserRole, TableAliasConstants.USER_ROLES_TABLE_ALIAS, parameters);
		}
		return parameters;
	}
}
