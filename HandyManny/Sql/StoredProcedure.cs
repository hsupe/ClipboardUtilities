using System;
using System.Text.RegularExpressions;

namespace Sql
{
	public class StoredProcedure
	{
		public static bool IsStoredProcedure(string Sql)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(PROCEDURE)\b[\w\W]*?\b(AS)\b";
			return Regex.Match(Sql, parameterDeclarationRegEx).Success;
		}

		public static string MakeDebuggable(string sql)
		{
			return new StoredProcedure(sql)
				.CommentOutCreateOrAlterLine()
				.CommentOutOUTPUTFromTheParameterDeclaration()
				.CommentOutASAtTheEndOfParameterDeclaration()
				.ChangeReturnStatementsToSelect()
				.Sql;
		}

		public StoredProcedure(string Sql)
		{
			this.Sql = Sql;
		}
		public string Sql;
	}

	internal static class StoredProcedureExtensions
	{
		// What is $&? 
		// Refer to https://docs.microsoft.com/en-us/dotnet/standard/base-types/substitutions-in-regular-expressions#EntireMatch
		public static StoredProcedure CommentOutCreateOrAlterLine(this StoredProcedure sp)
		{
			sp.Sql = Regex.Replace(sp.Sql, @"(?i)\b(ALTER|CREATE)\b\W+\b(PROCEDURE)\b.*(\r\n)", @"-- $&" + "declare" + Environment.NewLine);
			return sp;
		}

		public static StoredProcedure ChangeReturnStatementsToSelect(this StoredProcedure sp)
		{
			// See https://stackoverflow.com/questions/11620250/regex-match-keywords-that-are-not-in-quotes for how to match a word but not when it is not quoted.
			sp.Sql = Regex.Replace(sp.Sql, @"(?i)(?<=^([^\']|\'[^\']*\')*)\breturn\b[^\r]*", @"select '$&'");
			return sp;
		}

		public static StoredProcedure CommentOutOUTPUTFromTheParameterDeclaration(this StoredProcedure sp)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(PROCEDURE)\b[\w\W]*?\b(AS)\b";
			Match m = Regex.Match(sp.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)\boutput\b", @"/* $& */");
				sp.Sql = Regex.Replace(sp.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return sp;
		}

		public static StoredProcedure CommentOutASAtTheEndOfParameterDeclaration(this StoredProcedure sp)
		{
			string parameterDeclarationRegEx = @"(?i)\b(ALTER|CREATE)\b\W+\b(PROCEDURE|FUNCTION)\b[\w\W]*?\b(AS)\b";
			Match m = Regex.Match(sp.Sql, parameterDeclarationRegEx);
			if (m.Success)
			{
				string parameterDeclarationSql = m.Value;
				parameterDeclarationSql = Regex.Replace(parameterDeclarationSql, @"(?i)\bas\b", @"-- $& ");
				sp.Sql = Regex.Replace(sp.Sql, parameterDeclarationRegEx, parameterDeclarationSql);
			}
			return sp;
		}
	}
}
