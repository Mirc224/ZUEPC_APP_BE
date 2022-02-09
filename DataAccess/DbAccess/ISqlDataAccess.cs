
namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters, string connectionId = "Default");
	Task<T> ExecuteScalarAsync<T,U>(string sql, U parameters, string connectionId = "Default");
	Task<int> ExecuteAsync<T>(string sql, T parameters, string connectionId = "Default");
    Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
    Task UpdateData<T>(string storedProcedure, T parameters, string connectionId = "Default");
    Task DeleteData<T>(string storedProcedure, T parameters, string connectionId = "Default");
}