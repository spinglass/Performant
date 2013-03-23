using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TcpConnection;

namespace PerformantServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Create the notification icon
            m_NotifyIcon = new System.Windows.Forms.NotifyIcon();
            m_NotifyIcon.Icon = PerformantServer.Properties.Resources.DisconnectedIcon;
            m_NotifyIcon.Text = PerformantServer.Properties.Resources.DisconnectedTooltip;
            m_NotifyIcon.Visible = true;

            // Make menu
            System.Windows.Forms.MenuItem menuItem = new System.Windows.Forms.MenuItem("&Quit");
            menuItem.Click += Quit_Click;
            m_NotifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] { menuItem });

            // Start the server
            m_Server = new Server();
            m_Server.ConnectionChanged += Server_ConnectionChanged;
            m_Server.Start();
        }

        void Server_ConnectionChanged(object sender, PM3State state)
        {
            switch (state)
            {
                case PM3State.Connected:
                    m_NotifyIcon.Icon = PerformantServer.Properties.Resources.ConnectedIcon;
                    m_NotifyIcon.Text = PerformantServer.Properties.Resources.ConnectedTooltip;
                    break;
                case PM3State.Disconnected:
                    m_NotifyIcon.Icon = PerformantServer.Properties.Resources.DisconnectedIcon;
                    m_NotifyIcon.Text = PerformantServer.Properties.Resources.DisconnectedTooltip;
                    break;
            }
        }

        void Quit_Click(object sender, EventArgs e)
        {
            m_Server.Stop();
            Shutdown();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            m_NotifyIcon.Visible = false;
        }

        private System.Windows.Forms.NotifyIcon m_NotifyIcon;
        private Server m_Server;
    }
}
