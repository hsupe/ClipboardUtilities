using System;
using System.Windows.Forms;
using MutexManager;


// Most of this code is adapted from https://www.simple-talk.com/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
namespace HandyManny
{
    // Framework for restricting app to a single instance and for running as a tray app.
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!SingleInstance.Start()) { return; }
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
            SingleInstance.Stop();
        }
    }
}
