namespace Sql
{
	public class Utils
	{
		public static string MakeStoredProcedureOrFunctionDebuggable(string sql)
		{
			if (StoredProcedure.IsStoredProcedure(sql))
				return StoredProcedure.MakeDebuggable(sql);
			else if (Function.IsFunction(sql))
				return Function.MakeDebuggable(sql);
			else return sql;
		}
	}
}
