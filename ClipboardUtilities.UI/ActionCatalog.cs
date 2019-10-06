using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClipboardUtilities.UI
{
	public class ActionCatalog
	{
		// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
		private readonly object _actionImplementer;
		// ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

		private readonly Dictionary<string, ActionDelegate> _catalog;

		private delegate string ActionDelegate(string input);

		public ActionCatalog(object actionImplementer)
		{
			_actionImplementer = actionImplementer;
			_catalog = new Dictionary<string, ActionDelegate>();

			// TODO Refactor this into method.
			// TODO Get only methods matching 'string Name(string)' signature.
			var methodNames = _actionImplementer.GetType()
				.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Select(x => x.Name).ToList();

			methodNames.ForEach(x => _catalog.Add(x, MakeDelegate(x)));
			
			ActionDelegate MakeDelegate(string x)
			{
				return (ActionDelegate) Delegate.CreateDelegate(typeof(ActionDelegate),
					_actionImplementer, x);
			}
		}

		public List<string> GetUtilities() => _catalog.Keys.ToList();

		public string Invoke(string method, string input) => _catalog[method](input);
	}
}