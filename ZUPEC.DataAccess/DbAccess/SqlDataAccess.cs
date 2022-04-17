using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<int> ExecuteAsync<T>(
        string sql,
        T parameters,
        string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        return await connection.ExecuteAsync(sql,
                                             parameters,
                                             commandType: CommandType.Text);
    }


    public async Task<IEnumerable<T>> QueryAsync<T, U>(
        string sql,
        U parameters,
        string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryAsync<T>(sql,
                                              parameters,
                                              commandType: CommandType.Text);
    }

	public async Task<T> ExecuteScalarAsync<T, U>(string sql, U parameters, string connectionId = "Default")
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
		return await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: CommandType.Text);
	}
}
