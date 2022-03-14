
namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters, string connectionId = "Default");
	Task<T> ExecuteScalarAsync<T,U>(string sql, U parameters, string connectionId = "Default");
	Task<int> ExecuteAsync<T>(string sql, T parameters, string connectionId = "Default");
    Task SaveDataAsync<T>(string storedProcedure, T parameters, string connectionId = "Default");
    Task UpdateDataAsync<T>(string storedProcedure, T parameters, string connectionId = "Default");
    Task DeleteDataAsync<T>(string storedProcedure, T parameters, string connectionId = "Default");
}