using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClipboardUtilities.UI
{
	class ActionDiscoverer
	{
		private readonly object _actionImplementer;

		public ActionDiscoverer(object actionImplementer) => _actionImplementer = actionImplementer;

		public List<string> Discover()
		{
			var publicMethods = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

			return _actionImplementer.GetType()
				.GetMethods(publicMethods)
				.Where(MethodTakesStringAndReturnsString)
				.Select(x => x.Name).ToList();
		}

		private static bool MethodTakesStringAndReturnsString(MethodInfo m) => MethodTakesOnlyStringParameter(m) && MethodReturnsString(m);

		private static bool MethodReturnsString(MethodInfo m) => (m.ReturnType == typeof(string));

		private static bool MethodTakesOnlyStringParameter(MethodInfo m)
		{
			var parameters = m.GetParameters();
			return parameters.Length == 1 && parameters.First().ParameterType == typeof(string);
		}
	}
}