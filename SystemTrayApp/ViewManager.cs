using OpenALPR.SystemTrayApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemTrayApp.Properties;
using WpfFormLibrary.View;
using WpfFormLibrary.ViewModel;

namespace SystemTrayApp
{
    public class ViewManager:IDisposable
    {
        // This allows code to be run on a GUI thread
        private System.Windows.Window hiddenWindow;

        private IContainer components;
        // The Windows system tray class
        private NotifyIcon notifyIcon;
        private IServiceManager serviceManager;

        private AboutView aboutView;
        private AboutViewModel aboutViewModel;
        private StatusView statusView;
        private StatusViewModel statusViewModel;

        private ToolStripMenuItem startServiceMenuItem;
        private ToolStripMenuItem stopServiceMenuItem;
        private ToolStripMenuItem serviceAccessMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private bool disposed;

        public ViewManager(IServiceManager serviceManager)
        {
            Debug.Assert(serviceManager != null);

            this.serviceManager = serviceManager;

            components = new Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = serviceManager.Status == ServiceControllerStatus.Running ? Resources.shutter32x32BlueIcon : Resources.shutter32x32___GrayIcon,
                Text = serviceManager.Status.ToString (),
                Visible = true
            };

            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.MouseUp += NotifyIcon_MouseUp;

            aboutViewModel = new AboutViewModel();
            statusViewModel = new StatusViewModel { Icon = AppIcon };
            aboutViewModel.Icon = statusViewModel.Icon;

            hiddenWindow = new System.Windows.Window();
            hiddenWindow.Hide();
        }

        private ImageSource AppIcon
        {
            get
            {
                var icon = serviceManager.Status == ServiceControllerStatus.Running ? Resources.shutter32x32BlueIcon : Resources.shutter32x32___GrayIcon;
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void DisplayStatusMessage(string text)
        {
            hiddenWindow.Dispatcher.Invoke(() => 
            {
                notifyIcon.BalloonTipText = $"{serviceManager.ServiceName}: {text}";
                // The timeout is ignored on recent Windows
                notifyIcon.ShowBalloonTip(3000);
            });
        }

        public void OnStatusChange(object sender, EventArgs e)
        {
            UpdateStatusView();

            switch (serviceManager.Status)
            {
                case ServiceControllerStatus.Running:
                    notifyIcon.Text = $"{serviceManager.ServiceName}: Running";
                    notifyIcon.Icon = Resources.shutter32x32BlueIcon;
                    DisplayStatusMessage("Running");
                    break;

                case ServiceControllerStatus.StartPending:
                    notifyIcon.Text = $"{serviceManager.ServiceName}: Starting";
                    notifyIcon.Icon = Resources.shutter32x32BlueIcon;
                    DisplayStatusMessage("Starting");
                    break;

                case ServiceControllerStatus.Stopped:
                    notifyIcon.Text = $"{serviceManager.ServiceName}: Stopped";
                    notifyIcon.Icon = Resources.shutter32x32___GrayIcon;
                    DisplayStatusMessage("Stopped");
                    break;

                default:
                    notifyIcon.Text = $"{serviceManager.ServiceName}: -";
                    notifyIcon.Icon = Resources.shutter32x32___GrayIcon;
                    break;
            }

            if (aboutView != null)
                aboutView.Icon = AppIcon;
 
            if (statusView != null)
                statusView.Icon = AppIcon;
        }

        private void UpdateStatusView()
        {
            if ((statusViewModel != null) && (serviceManager != null))
            {
                var flags = serviceManager.StatusFlags;
                var statusItems = flags.Select(n => new KeyValuePair<string, string>(n.Key, n.Value.ToString())).ToList();
                statusItems.Insert(0, new KeyValuePair<string, string>("OpenALPRMilestone Service ", serviceManager.Status.ToString()));
                statusViewModel.SetStatusFlags(statusItems);
            }
        }

        private void StartStopReaderItem_Click(object sender, EventArgs e)
        {
            if (serviceManager.Status == ServiceControllerStatus.Running)
                serviceManager.Stop();
            else
                serviceManager.Start();
        }

        private void RunServiceAccessScript_Click(object sender, EventArgs e)
        {
            if (serviceManager.Status == ServiceControllerStatus.Running)
                serviceManager.Stop();

            string file = $"{Application.StartupPath}\\service_access.ps1";

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0\\powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Unrestricted -File \"{file}\"",
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(startInfo);
        }

        private void RunRestartRecordingServerScript_Click(object sender, EventArgs e)
        {
            if (serviceManager.Status == ServiceControllerStatus.Running)
                serviceManager.Stop();

            string file = $"{Application.StartupPath}\\recording_server.ps1";

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0\\powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Unrestricted -File \"{file}\"",
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(startInfo);
        }

        private ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string tooltipText, Image image, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText, image);
            if (eventHandler != null)
                item.Click += eventHandler;

            item.ToolTipText = tooltipText;
            return item;
        }
        
        private void ShowStatusView()
        {
            if (statusView == null)
            {
                statusView = new StatusView { DataContext = statusViewModel };

                statusView.Closing += ((arg_1, arg_2) => statusView = null);
                statusView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                statusView.Show();
                UpdateStatusView();
            }
            else
            {
                statusView.Activate();
            }

            statusView.Icon = AppIcon;
        }

        private void ShowStatusItem_Click(object sender, EventArgs e)
        {
            ShowStatusView();
        }

        private void ShowAboutView()
        {
            if (aboutView == null)
            {
                aboutView = new AboutView { DataContext = aboutViewModel };
                aboutView.Closing += ((arg_1, arg_2) => aboutView = null);
                aboutView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                aboutView.Show();
            }
            else
            {
                aboutView.Activate();
            }
            aboutView.Icon = AppIcon;

            //aboutViewModel.AddVersionInfo("Service", serviceManager.ServiceName);
            aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //aboutViewModel.AddVersionInfo("Serial Number", "142573462354");
        }

        private void ShowAlerListing_Click(object sender, EventArgs e)
        {
            ShowAlerListing();
        }

        private void ShowAlerListing()
        {
            const string PlugName = "OpenALPR";

            try
            {
                var mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Mapping");

                if (!Directory.Exists(mappingPath))
                {
                    Directory.CreateDirectory(mappingPath);
                    Helper.SetDirectoryNetworkServiceAccessControl(mappingPath);
                }

                var filePath = Path.Combine(mappingPath, "AlertList.txt");

                if (!File.Exists(filePath))
                {
                    using (var outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine("# Edit this file to add alerts.");
                        outputFile.WriteLine("# Each line represents one alert and a description separated by a comma.");
                        outputFile.WriteLine("# For example (Do not use a \"#\" symbol for your alerts):");
                        outputFile.WriteLine("# ABC123,Walter Smith's Truck");
                        outputFile.WriteLine("Plate Number, Description\n");
                    }
                }

                if (File.Exists(filePath))
                    Process.Start("explorer.exe", filePath);
            }
            catch //(Exception ex)
            {
                //Logger.Log.Error(null, ex);
            }
        }

        private void  OpenLogFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", @"C:\ProgramData\OpenALPR\Log");
            }
            catch //(Exception ex)
            {
                //Logger.Log.Error(null, ex);
            }
        }

        private void ShowHelpItem_Click(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void ShowWebSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://doc.openalpr.com/");
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            string msg = $"Are you sure you wish to exit ?";
            var result = MessageBox.Show(msg, "OpenALPRMilestone", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }
        
        private void SetMenuItems()
        {
            switch (serviceManager.Status)
            {
                case ServiceControllerStatus.StartPending:
                    startServiceMenuItem.Enabled = false;
                    stopServiceMenuItem.Enabled = false;
                    exitMenuItem.Enabled = false;
                    break;

                case ServiceControllerStatus.Running:
                    startServiceMenuItem.Enabled = false;
                    stopServiceMenuItem.Enabled = true;
                    exitMenuItem.Enabled = true;
                    break;

                case ServiceControllerStatus.Stopped:
                    startServiceMenuItem.Enabled = true;
                    stopServiceMenuItem.Enabled = false;
                    exitMenuItem.Enabled = true;
                    break;

                default:
                    Debug.Assert(false, "SetButtonStatus() => Unknown state");
                    break;
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            if (notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                startServiceMenuItem = ToolStripMenuItemWithHandler("Start OpenALPRMilestone service", "Starts the service", Resources.Start, StartStopReaderItem_Click);
                notifyIcon.ContextMenuStrip.Items.Add(startServiceMenuItem);
                stopServiceMenuItem = ToolStripMenuItemWithHandler("Stop OpenALPRMilestone service", "Stops the service", Resources.Stop, StartStopReaderItem_Click);
                notifyIcon.ContextMenuStrip.Items.Add(stopServiceMenuItem);
                notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                serviceAccessMenuItem = ToolStripMenuItemWithHandler("Run service access script", "Run service access script", Resources.powershell, RunServiceAccessScript_Click);
                notifyIcon.ContextMenuStrip.Items.Add(serviceAccessMenuItem);
                serviceAccessMenuItem = ToolStripMenuItemWithHandler("Restart Xprotect Recording Server", "Restart Xprotect Recording Server", Resources.Recorder, RunRestartRecordingServerScript_Click);
                notifyIcon.ContextMenuStrip.Items.Add(serviceAccessMenuItem);
                notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("OpenALPRMilestone service s&tatus", "Shows the service status dialog", Resources.QuestionMark, ShowStatusItem_Click));

                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Alert &listing", "Open Alert listing file", Resources.QuestionMark, ShowAlerListing_Click));
                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Open lo&g", "Open log folder", Resources.folder, OpenLogFolder_Click));


                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&About", "Shows the About dialog", Resources.shutter32x32, ShowHelpItem_Click));
                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("OpenALPRMilestone &web site", "Navigates to the OpenALPR Web Site", Resources.shutter32x32, ShowWebSite_Click));
                notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

                exitMenuItem = ToolStripMenuItemWithHandler("&Exit", "Exits OpenALPRMilestone", Resources.Exit, ExitItem_Click);
                notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
            }

            SetMenuItems();
        }

        #region Disposable Methods

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                
                    Close();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.


                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion


        private void Close()
        {
            if (startServiceMenuItem != null)
            {
                startServiceMenuItem.Dispose ();
                startServiceMenuItem = null;
            }

            if (stopServiceMenuItem != null)
            {
                stopServiceMenuItem.Dispose();
                stopServiceMenuItem = null;
            }

            if (exitMenuItem != null)
            {
                exitMenuItem.Dispose ();
                exitMenuItem = null;
            }

            if (notifyIcon != null)
            {
                notifyIcon.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
                notifyIcon.DoubleClick -= NotifyIcon_DoubleClick;
                notifyIcon.MouseUp -= NotifyIcon_MouseUp;

                notifyIcon.Dispose();
                notifyIcon = null;
            }

            if (aboutViewModel != null)
            {
                aboutViewModel.Dispose();
                aboutViewModel = null;
            }

            if (statusViewModel != null)
            {
                statusViewModel.Dispose();
                statusViewModel = null;
            }

            if (hiddenWindow != null)
            {
                hiddenWindow.Close();
                hiddenWindow = null;
            }

            if(components != null)
            {
                components.Dispose();
                components = null;
            }
        }
    }
}
