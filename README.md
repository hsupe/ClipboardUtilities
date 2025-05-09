# ClipboardUtilities

This repo represents how the idea started in my head before I begun using VSCode or had a license for tools like SQLPrompt which provide many of similar utilities. I started using this application at work, it spread among the team members and a more actively maintained and expanded version was created on the work repo including several utilities to decode some of complex data blobs for production troubleshooting.

## Overview

ClipboardUtilities is a system tray application that provides several clipboard utilities like Sort, Reverse, Remove duplicates, construct SQL IN clause and so on. This vaguely follows the Unix philosophy - building software as a collection of small, focused, and composable tools that do one thing well. The individual utilities can be chained through clipboard (similar to the pipe, stdin, stdout concept).

When you run the application it creates an icon in the system tray. Right clicking the icon, brings up menu with options each of which performs the given operation on the contents in Windows clipboard and puts the result back into the clipboard.

![ClipboardUtilities](https://github.com/hlsupe/ClipboardUtilities/blob/master/ClipboardUtilities.png)

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
