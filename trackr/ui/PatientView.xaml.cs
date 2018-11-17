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

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : Page
    {
        string patient;

        public PatientView()
        {
            InitializeComponent();
        }

        public PatientView(object o)
        {
            InitializeComponent();

            patient = (string)o;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock.Dispatcher.BeginInvoke((Action)delegate ()
            {
                this.textBlock.Text = patient;
            });
        }
    }
}
