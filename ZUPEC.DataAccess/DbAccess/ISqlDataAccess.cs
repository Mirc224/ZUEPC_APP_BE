
namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters, string connectionId = "Default");
	Task<T> ExecuteScalarAsync<T,U>(string sql, U parameters, string connectionId = "Default");
	Task<int> ExecuteAsync<T>(string sql, T parameters, string connectionId = "Default");
}