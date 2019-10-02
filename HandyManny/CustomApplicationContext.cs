using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HandyManny
{
    // Framework for running application as a tray app.

    // Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    // http://windowsclient.net/articles/notifyiconapplications.aspx
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "route.ico";
        private static readonly string DefaultTooltip = "HandyManny";
        private readonly ActionManager ActionManager;
       
		// This class should be created and passed into Application.Run( ... )
		public CustomApplicationContext() 
		{
			InitializeContext();
            ActionManager = new ActionManager(notifyIcon);
		}

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            ActionManager.BuildContextMenu(notifyIcon.ContextMenuStrip);
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(ActionManager.ToolStripMenuItemWithHandler("&Exit", exitItem_Click));
        }

        // From http://stackoverflow.com/questions/2208690/invoke-notifyicons-context-menu
        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        # region generic code framework

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
                             {
                                 ContextMenuStrip = new ContextMenuStrip(),
                                 Icon = new Icon(IconFileName),
                                 Text = DefaultTooltip,
                                 Visible = true
                             };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            //notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        
		// When the application context is disposed, dispose things like the notify icon.
		protected override void Dispose( bool disposing )
		{
			if( disposing && components != null) { components.Dispose(); }
		}

		
		// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
		private void exitItem_Click(object sender, EventArgs e) 
		{
			ExitThread();
		}

       
        // If we are presently showing a form, clean it up.
        protected override void ExitThreadCore()
        {
            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        # endregion generic code framework

    }
}
