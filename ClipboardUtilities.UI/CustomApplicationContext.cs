using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ClipboardUtilities.UI
{
    // Framework for running application as a tray app.

    // Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    // http://windowsclient.net/articles/notifyiconapplications.aspx
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "route.ico";
        private static readonly string DefaultTooltip = "ClipboardUtilities";
        private readonly ActionManager _actionManager;
       
		// This class should be created and passed into Application.Run( ... )
		public CustomApplicationContext() 
		{
			InitializeContext();
			// TODO We should be able to add more than one ActionCatalogs to actionManager. For example string catalog, math catalog, set catalog and so on.
			var catalog = new ActionCatalog(new Lib.StringUtilities());
			_actionManager = new ActionManager(catalog);
		}

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            _actionManager.BuildContextMenu(_notifyIcon.ContextMenuStrip);
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            _notifyIcon.ContextMenuStrip.Items.Add(_actionManager.ToolStripMenuItemWithHandler("&Exit", exitItem_Click));
        }

        // From http://stackoverflow.com/questions/2208690/invoke-notifyicons-context-menu
        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
	            MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
	            if (mi != null) mi.Invoke(_notifyIcon, null);
            }
        }

        # region generic code framework

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon _notifyIcon;				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            _notifyIcon = new NotifyIcon(components)
                             {
                                 ContextMenuStrip = new ContextMenuStrip(),
                                 Icon = new Icon(IconFileName),
                                 Text = DefaultTooltip,
                                 Visible = true
                             };
            _notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            //notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            _notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        
		// When the application context is disposed, dispose things like the notify icon.
		protected override void Dispose( bool disposing )
		{
			if( disposing) { components?.Dispose(); }
		}

		
		// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
		private void exitItem_Click(object sender, EventArgs e) 
		{
			ExitThread();
		}

       
        // If we are presently showing a form, clean it up.
        protected override void ExitThreadCore()
        {
            _notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        # endregion generic code framework

    }
}
