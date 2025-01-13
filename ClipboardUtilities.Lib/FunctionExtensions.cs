using System;
using System.Text.RegularExpressions;

namespace ClipboardUtilities.Lib;

public class Utils
{
	public static string MakeStoredProcedureOrFunctionDebuggable(string sql)
	{
		if (StoredProcedure.IsStoredProcedure(sql))
			return StoredProcedure.MakeDebuggable(sql);
		return Function.IsFunction(sql) ? Function.MakeDebuggable(sql) : sql;
	}
}

internal static class FunctionExtensions
{
	public static Function CommentOutCreateOrAlterLine(this Function fn)
	{
		fn.Sql = Regex.Replace(fn.Sql, "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b.*(\\r\\n)",
			"-- $&declare" + Environment.NewLine);
		return fn;
	}

	public static Function ChangeReturnStatementsToSelect(this Function fn)
	{
		fn.Sql = Regex.Replace(fn.Sql, "(?i)(?<=^([^\\']|\\'[^\\']*\\')*)\\breturn\\b[^\\r]*", "select '$&'");
		return fn;
	}

	public static Function CommentOutOUTPUTFromTheParameterDeclaration(this Function fn)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b[\\w\\W]*?\\b(RETURNS)\\b";
		var match = Regex.Match(fn.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)\\boutput\\b", "/* $& */");
			fn.Sql = Regex.Replace(fn.Sql, pattern, replacement);
		}

		return fn;
	}

	public static Function CommentOutParenthesisFromTheParameterDeclaration(this Function fn)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b[\\w\\W]*?\\b(RETURNS)\\b";
		var match = Regex.Match(fn.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)(?m)^\\W*[\\(|\\)]", "-- $& ");
			fn.Sql = Regex.Replace(fn.Sql, pattern, replacement);
		}

		return fn;
	}

	public static Function ChangeReturnsToDeclareInFunctionSignature(this Function fn)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b[\\w\\W]*?\\b(RETURNS)\\b";
		var match = Regex.Match(fn.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)\\bRETURNS\\b", "declare");
			fn.Sql = Regex.Replace(fn.Sql, pattern, replacement);
		}

		return fn;
	}

	public static Function CommentOutASFromFunctionSignature(this Function fn)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b[\\w\\W]*?\\b(AS)\\b";
		var match = Regex.Match(fn.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)\\bAS\\b", "-- $&");
			fn.Sql = Regex.Replace(fn.Sql, pattern, replacement);
		}

		return fn;
	}
}

internal class Function
{
	public string Sql;

	public Function(string Sql)
	{
		this.Sql = Sql;
	}

	public static bool IsFunction(string Sql)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(FUNCTION)\\b[\\w\\W]*?\\b(RETURNS)\\b";
		return Regex.Match(Sql, pattern).Success;
	}

	public static string MakeDebuggable(string sql)
	{
		return new Function(sql).CommentOutCreateOrAlterLine().CommentOutOUTPUTFromTheParameterDeclaration()
			.CommentOutParenthesisFromTheParameterDeclaration().ChangeReturnsToDeclareInFunctionSignature()
			.ChangeReturnStatementsToSelect().CommentOutASFromFunctionSignature().Sql;
	}
}

public class StoredProcedure
{
	public string Sql;

	public StoredProcedure(string Sql)
	{
		this.Sql = Sql;
	}

	public static bool IsStoredProcedure(string Sql)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(PROCEDURE)\\b[\\w\\W]*?\\b(AS)\\b";
		return Regex.Match(Sql, pattern).Success;
	}

	public static string MakeDebuggable(string sql)
	{
		return new StoredProcedure(sql).CommentOutCreateOrAlterLine().CommentOutOUTPUTFromTheParameterDeclaration()
			.CommentOutASAtTheEndOfParameterDeclaration().ChangeReturnStatementsToSelect().Sql;
	}
}

internal static class StoredProcedureExtensions
{
	public static StoredProcedure CommentOutCreateOrAlterLine(this StoredProcedure sp)
	{
		sp.Sql = Regex.Replace(sp.Sql, "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(PROCEDURE)\\b.*(\\r\\n)",
			"-- $&declare" + Environment.NewLine);
		return sp;
	}

	public static StoredProcedure ChangeReturnStatementsToSelect(this StoredProcedure sp)
	{
		sp.Sql = Regex.Replace(sp.Sql, "(?i)(?<=^([^\\']|\\'[^\\']*\\')*)\\breturn\\b[^\\r]*", "select '$&'");
		return sp;
	}

	public static StoredProcedure CommentOutOUTPUTFromTheParameterDeclaration(
		this StoredProcedure sp)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(PROCEDURE)\\b[\\w\\W]*?\\b(AS)\\b";
		var match = Regex.Match(sp.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)\\boutput\\b", "/* $& */");
			sp.Sql = Regex.Replace(sp.Sql, pattern, replacement);
		}

		return sp;
	}

	public static StoredProcedure CommentOutASAtTheEndOfParameterDeclaration(this StoredProcedure sp)
	{
		var pattern = "(?i)\\b(ALTER|CREATE)\\b\\W+\\b(PROCEDURE|FUNCTION)\\b[\\w\\W]*?\\b(AS)\\b";
		var match = Regex.Match(sp.Sql, pattern);
		if (match.Success)
		{
			var replacement = Regex.Replace(match.Value, "(?i)\\bas\\b", "-- $& ");
			sp.Sql = Regex.Replace(sp.Sql, pattern, replacement);
		}

		return sp;
	}
}