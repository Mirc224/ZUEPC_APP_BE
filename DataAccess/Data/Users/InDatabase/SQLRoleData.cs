using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLRoleData :
	SQLDbRepositoryBase<RoleModel>, IRoleData
{
	public SQLRoleData(ISqlDataAccess db) 
		: base(db, TableNameConstants.ROLES_TABLE, TableAliasConstants.ROLES_TABLE_ALIAS)
	{
	}
}
