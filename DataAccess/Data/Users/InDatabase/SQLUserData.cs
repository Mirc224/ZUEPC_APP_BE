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
	SQLDbRepositoryWithFilterBase<IUserData, UserModel, UserFilter>, 
	IUserData
{
	public SQLUserData(ISqlDataAccess db)
	: base(db, TableNameConstants.USERS_TABLE, TableAliasConstants.USERS_TABLE_ALIAS) 
	{
	}

	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(UserModel.Email), email, builder, parameters);
		return (await GetModelsAsync(parameters, builder)).FirstOrDefault();
	}

	protected override dynamic BuildJoinWithFilterExpression(UserFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if(queryFilter.UserRole != null)
		{
			AddToLeftJoinExpression(
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
			builder.WhereInArray(nameof(UserModel.Email), queryFilter.Email, baseTableAlias, parameters);
		}
		if (queryFilter.FirstName != null)
		{
			builder.WhereInArray(nameof(UserModel.FirstName), queryFilter.FirstName, baseTableAlias, parameters);
		}
		if (queryFilter.LastName != null)
		{
			builder.WhereInArray(nameof(UserModel.LastName), queryFilter.LastName, baseTableAlias, parameters);
		}
		if (queryFilter.UserRole != null)
		{
			builder.WhereInArray(nameof(UserRoleModel.RoleId), queryFilter.UserRole, TableAliasConstants.USER_ROLES_TABLE_ALIAS, parameters);
		}
		return parameters;
	}
}
