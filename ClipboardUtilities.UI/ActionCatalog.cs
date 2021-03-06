﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ClipboardUtilities.UI
{
	//TODO Add Unit Tests
	public class ActionCatalog
	{
		private Dictionary<string, ActionDelegate> _catalog;

		private delegate string ActionDelegate(string input);

		public void Add(Object actionImplementer)
		{
			BuildCatalog(actionImplementer);
		}

		private void BuildCatalog(object actionImplementer)
		{
			if(_catalog == null )
				_catalog = new Dictionary<string, ActionDelegate>();

			new ActionDiscoverer(actionImplementer)
				.Discover()
				.ForEach(x => _catalog.Add(x, MakeDelegate(x, actionImplementer)));
		}

		private ActionDelegate MakeDelegate(string nameOfMethod, object actionImplementer) =>
			(ActionDelegate) Delegate.CreateDelegate(typeof(ActionDelegate), actionImplementer, nameOfMethod);

		public List<string> Actions() => _catalog.Keys.ToList();

		public string InvokeAction(string nameOfAction, string input) => _catalog[nameOfAction](input);
	}
}