using System;
using System.Collections.Generic;
using System.Linq;

namespace ClipboardUtilities.UI
{
	public class ActionCatalog
	{
		// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
		private readonly object _actionImplementer;
		// ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

		private Dictionary<string, ActionDelegate> _catalog;

		private delegate string ActionDelegate(string input);

		public ActionCatalog(object actionImplementer)
		{
			_actionImplementer = actionImplementer;
			BuildCatalog();
		}

		private void BuildCatalog()
		{
			_catalog = new Dictionary<string, ActionDelegate>();
			new ActionDiscoverer(_actionImplementer).Discover().ForEach(x => _catalog.Add(x, MakeDelegate(x)));
		}

		private ActionDelegate MakeDelegate(string x) => (ActionDelegate) Delegate.CreateDelegate(typeof(ActionDelegate), _actionImplementer, x);

		public List<string> Actions() => _catalog.Keys.ToList();

		public string InvokeAction(string nameOfAction, string input) => _catalog[nameOfAction](input);
	}
}