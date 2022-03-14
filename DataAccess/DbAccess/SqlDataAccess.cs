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

    public async Task<IEnumerable<T>> LoadDataAsync<T, U>(
        string storedProcedure,
        U parameters,
        string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(storedProcedure,
                                              parameters,
                                              commandType: CommandType.StoredProcedure);

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

    public async Task SaveDataAsync<T>(
        string storedProcedure,
        T parameters,
        string connectionId = "Default")
    {
        await ExecuteNonQueryProcedure(storedProcedure, parameters, connectionId);
    }

    public async Task UpdateDataAsync<T>(
        string storedProcedure,
        T parameters,
        string connectionId = "Default")
    {
        await ExecuteNonQueryProcedure(storedProcedure, parameters, connectionId);
    }

    public async Task DeleteDataAsync<T>(string storedProcedure, T parameters, string connectionId = "Default")
    {
        await ExecuteNonQueryProcedure(storedProcedure, parameters, connectionId);
    }

    private async Task ExecuteNonQueryProcedure<T>(
        string storedProcedure,
        T parameters,
        string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }

	public async Task<T> ExecuteScalarAsync<T, U>(string sql, U parameters, string connectionId = "Default")
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
		return await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: CommandType.Text);
	}
}
