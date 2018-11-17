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
using trackr.core;
using trackr.imgProcessing;

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Workspace.Instance.Init();
            CameraController.InitializeToDefaultCamera();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Workspace.Instance.Close();           
        }
    }
}
