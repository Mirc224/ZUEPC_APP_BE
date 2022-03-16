using Dapper;
using System.Dynamic;

namespace ZUEPC.DataAccess.Extensions;

public static class SqlBuilderExt
{
	public static ExpandoObject WhereInArray<T>(this SqlBuilder builder, string keyName, IEnumerable<T> values, string? alias)
	{
		ExpandoObject parameters = new();
		builder.WhereInArray(keyName, values, alias, parameters);
		return parameters;
	}

	public static void WhereInArray<T>(this SqlBuilder builder, string keyName, IEnumerable<T> values, string? alias, ExpandoObject parameters)
	{
		if (alias != null)
		{
			alias = alias.Trim();
		}
		if (string.IsNullOrEmpty(alias))
		{
			alias = "";
		}
		int counter = 1;
		List<string> innerValues = new();
		foreach (T item in values)
		{
			string paramName = $"{alias}{keyName}{counter++}";
			innerValues.Add("@" + paramName);
			parameters.TryAdd(paramName, item);
		}
		string resultInner = string.Join(',', innerValues);
		string columnAlias = alias;
		if(!string.IsNullOrEmpty(columnAlias))
		{
			columnAlias = columnAlias + ".";
		}
		builder.Where($"{columnAlias}{keyName} IN ({resultInner})");
	}

	public static ExpandoObject WhereLikeInArray(this SqlBuilder builder, string keyName, IEnumerable<string> values, string? alias)
	{
		ExpandoObject parameters = new();
		builder.WhereLikeInArray(keyName, values, alias, parameters);
		return parameters;
	}

	public static void WhereLikeInArray(this SqlBuilder builder, string keyName, IEnumerable<string> values, string? alias, ExpandoObject parameters)
	{
		if (alias != null)
		{
			alias = alias.Trim();
		}
		if (string.IsNullOrEmpty(alias))
		{
			alias = "";
		}
		int counter = 1;
		string columnAlias = alias;
		if (!string.IsNullOrEmpty(columnAlias))
		{
			columnAlias = columnAlias + ".";
		}
		List<string> innerValues = new();
		foreach (string item in values)
		{
			string paramName = $"{alias}{keyName}{counter++}";

			innerValues.Add($"{columnAlias}{keyName} LIKE @{paramName}");
			string term = $"%{item}%";
			parameters.TryAdd(paramName, term);
		}
		string resultInner = string.Join(" OR ", innerValues);

		builder.Where($"({resultInner})");
	}
}
