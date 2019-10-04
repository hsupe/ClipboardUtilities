using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;


// Most of this code is adapted from https://www.simple-talk.com/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
namespace HandyManny
{
	// Framework for restricting app to a single instance and for running as a tray app.
	static class Program
	{
		[STAThread]
		static void Main()
		{
			if (InstanceAlreadyRunning())
			{
				MessageBox.Show("Program already running.", Process.GetCurrentProcess().ProcessName);
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				var applicationContext = new CustomApplicationContext();
				Application.Run(applicationContext);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Program Terminated Unexpectedly",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static bool InstanceAlreadyRunning()
		{
			// This is not perfect Mutex - if two instances launch exactly same time, they may see each other running and both quit - live locking.
			// But this is simple and serves the purpose quite well.

			string thisProcessName = Process.GetCurrentProcess().ProcessName;
			return Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1;
		}
	}
}