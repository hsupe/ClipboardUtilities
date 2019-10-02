using System;
using System.Text.RegularExpressions;

namespace Sql
{
	internal class Function
	{
		public static bool IsFunction(string Sql)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b[\w\W]*?\b(RETURNS)\b";
			return Regex.Match(Sql, parameterDeclarationRegEx).Success;
		}

		public static string MakeDebuggable(string sql)
		{
			return new Function(sql)
				.CommentOutCreateOrAlterLine()
				.CommentOutOUTPUTFromTheParameterDeclaration()
				.CommentOutParenthesisFromTheParameterDeclaration()
				.ChangeReturnsToDeclareInFunctionSignature()
				.ChangeReturnStatementsToSelect()
				.CommentOutASFromFunctionSignature()
				.Sql;
		}
		public Function(string Sql)
		{
			this.Sql = Sql;
		}
		public string Sql;
	}

	internal static class FunctionExtensions
	{
		// What is $&? 
		// Refer to https://docs.microsoft.com/en-us/dotnet/standard/base-types/substitutions-in-regular-expressions#EntireMatch
		public static Function CommentOutCreateOrAlterLine(this Function fn)
		{
			fn.Sql = Regex.Replace(fn.Sql, @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b.*(\r\n)", @"-- $&" + "declare" + Environment.NewLine);
			return fn;
		}

		public static Function ChangeReturnStatementsToSelect(this Function fn)
		{
			// See https://stackoverflow.com/questions/11620250/regex-match-keywords-that-are-not-in-quotes for how to match a word but not when it is not quoted.
			fn.Sql = Regex.Replace(fn.Sql, @"(?i)(?<=^([^\']|\'[^\']*\')*)\breturn\b[^\r]*", @"select '$&'");
			return fn;
		}

		public static Function CommentOutOUTPUTFromTheParameterDeclaration(this Function fn)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b[\w\W]*?\b(RETURNS)\b";
			Match m = Regex.Match(fn.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)\boutput\b", @"/* $& */");
				fn.Sql = Regex.Replace(fn.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return fn;
		}

		public static Function CommentOutParenthesisFromTheParameterDeclaration(this Function fn)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b[\w\W]*?\b(RETURNS)\b";
			Match m = Regex.Match(fn.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)(?m)^\W*[\(|\)]", @"-- $& ");
				fn.Sql = Regex.Replace(fn.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return fn;
		}

		public static Function ChangeReturnsToDeclareInFunctionSignature(this Function fn)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b[\w\W]*?\b(RETURNS)\b";
			Match m = Regex.Match(fn.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)\bRETURNS\b", @"declare");
				fn.Sql = Regex.Replace(fn.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return fn;
		}

		public static Function CommentOutASFromFunctionSignature(this Function fn)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(FUNCTION)\b[\w\W]*?\b(AS)\b";
			Match m = Regex.Match(fn.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)\bAS\b", @"-- $&");
				fn.Sql = Regex.Replace(fn.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return fn;
		}
	}
}
