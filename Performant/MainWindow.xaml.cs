﻿using Monitor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            m_Controller = Controller.CreateDirect();
            m_StateView = new StateView();
            m_StateWatcher = new StateWatcher(Dispatcher, m_StateView, m_Controller);

            DataContext = m_StateView;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_Controller.Start();
            m_StateWatcher.Start();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            m_StateWatcher.Stop();
            m_Controller.Stop();
        }

        private Controller m_Controller;
        private StateView m_StateView;
        private StateWatcher m_StateWatcher;
    }
}
