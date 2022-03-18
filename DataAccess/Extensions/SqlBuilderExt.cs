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
		if (!innerValues.Any())
		{
			return;
		}
		string resultInner = string.Join(',', innerValues);
		string columnAlias = alias;
		if(!string.IsNullOrEmpty(columnAlias))
		{
			columnAlias = columnAlias + ".";
		}
		builder.Where($"{columnAlias}{keyName} IN ({resultInner})");
	}


	public static ExpandoObject WhereLikeInArray(this SqlBuilder builder, string keyName, IEnumerable<string> values, string? keyAlias)
	{
		ExpandoObject parameters = new();
		builder.WhereLikeInArray(keyName, values, keyAlias, parameters);
		return parameters;
	}

	public static string WhereLikeInArray(this SqlBuilder builder, string keyName, IEnumerable<string> values, string? keyAlias, ExpandoObject parameters)
	{
		string result = builder.WhereLikeInArrayBindedString(keyName, values, keyAlias, parameters);
		builder.Where($"({result})");
		return result;
	}

	public static string WhereLikeInArrayBindedString(this SqlBuilder builder, string keyName, IEnumerable<string> values, string? keyAlias, ExpandoObject parameters)
	{
		if (keyAlias != null)
		{
			keyAlias = keyAlias.Trim();
		}
		if (string.IsNullOrEmpty(keyAlias))
		{
			keyAlias = "";
		}

		string columnAlias = keyAlias;
		if (!string.IsNullOrEmpty(columnAlias))
		{
			columnAlias = columnAlias + ".";
		}
		List<string> innerValues = new();
		foreach (string item in values)
		{
			string paramName = Guid.NewGuid().ToString("N"); ;

			innerValues.Add($"{columnAlias}{keyName} LIKE @{paramName}");
			string term = $"%{item}%";
			parameters.TryAdd(paramName, term);
		}
		if (!innerValues.Any())
		{
			return "";
		}
		string resultInner = string.Join(" OR ", innerValues);
		return $"{resultInner}";
	}

	public static string GetConcatFunctionString(
		this SqlBuilder builder,
		string firstKeyName,
		string secondKeyName,
		string? firstKeyAlias,
		string? secondKeyAlias,
		ExpandoObject parameters,
		string delimeter = " ")
	{
		if(firstKeyAlias != null)
		{
			firstKeyAlias +=".";
		}
		if (secondKeyAlias != null)
		{
			secondKeyAlias += ".";
		}
		if (firstKeyAlias is null)
		{
			firstKeyAlias = "";
		}
		if (secondKeyAlias is null)
		{
			secondKeyAlias = "";
		}
		string delimeterAlias = "@" + Guid.NewGuid().ToString("N");
		parameters.TryAdd(delimeterAlias, delimeter);
		return $"CONCAT({firstKeyAlias}{firstKeyName},{delimeterAlias},{secondKeyAlias}{secondKeyName})";
	}

	public static string GetConcatFunctionString(
		this SqlBuilder builder,
		string firstKeyName,
		string secondKeyName,
		string? alias,
		ExpandoObject parameters,
		string delimeter = " ")
	{
		
		return builder.GetConcatFunctionString(firstKeyName, secondKeyName, alias, alias, parameters, delimeter);
	}
}
