using Dapper;
using System.Dynamic;

namespace ZUEPC.DataAccess.Extensions;

public static class SqlBuilderExt
{
	public static void WhereInArray<T>(this SqlBuilder builder, string keyName, IEnumerable<T> values, string? alias)
	{
		if(alias != null)
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
			innerValues.Add("@"+paramName);
		}
		string resultInner = string.Join(',', innerValues);
		string columnAlias = alias;
		if (!string.IsNullOrEmpty(columnAlias))
		{
			columnAlias = columnAlias + ".";
		}
		builder.Where($"{columnAlias}{keyName} IN ({resultInner})");
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
}
