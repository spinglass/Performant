using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Performant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            m_Monitor = new Monitor.Monitor();
            m_StateView = new StateView();
            m_StateWatcher = new StateWatcher(Dispatcher, m_StateView, m_Monitor);

            DataContext = m_StateView;
        }

        //public StateView State { get { return m_StateView; } }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_Monitor.Start();
            m_StateWatcher.Start();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            m_StateWatcher.Stop();
            m_Monitor.Stop();
        }

        private Monitor.Monitor m_Monitor;
        private StateView m_StateView;
        private StateWatcher m_StateWatcher;
    }
}
