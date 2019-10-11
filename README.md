# ClipboardUtilities

## Overview

ClipboardUtilities is a system tray application that provides several clipboard utilities like Sort, Reverse, Remove duplicates, construct SQL IN clause and so on.

When you run the application it creates an icon in the system tray. Right clicking the icon, brings up menu with options each of which performs the given operation on the contents in Windows clipboard and puts the result back into the clipboard.

![ClipboardUtilities](master/ClipboardUtilities.png)

The installer is not available yet, but copying binaries and running the executable is sufficient.

## How to add new actions?
1. Define a new set of utility actions, similar to StringUtilities, in ClipboardUtilities.Lib
2. Modify ClipboardUtilities.UI\CustomApplicationContext.cs 

```CSharp
		public CustomApplicationContext() 
		{
			InitializeContext();
			
			var catalog = new ActionCatalog();
			catalog.Add(new Lib.StringUtilities());
			catalog.Add(new Lib.YourNewUtilities());	// <= Inject your utiities.
			_actionManager = new ActionManager(catalog);
		}
```

The ActionCatalog.Add method will run reflection on Lib.YourNewUtilities instance to extract methods matching to 

```Csharp
		public string AnyName(string input);
```
and add those to the ClipboardUtilities menu.
