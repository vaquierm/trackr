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
using trackr.core;

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : Page
    {
        public PatientView()
        {
            InitializeComponent();
        }

        public PatientView(object o)
        {
            this.DataContext = PatientViewViewModel.Instance;

            var patient = Workspace.Instance.GetTherapyPatientFromStringId((string)o);

            PatientViewViewModel.Instance.ActivePatient = patient;

            InitializeComponent();

            chart.UpdateSeriesPatientBasis(patient);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PatientViewViewModel.Instance.ActivePatient = null;

            NavigationService.GoBack();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ImageFeedWindow imageFeedWindow = new ImageFeedWindow();
            imageFeedWindow.Show();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            PatientViewViewModel.Instance.StartNewSession(); 
        }
        
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            PatientViewViewModel.Instance.EndCurrentSession();
        }
    }
}
