using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Workspace.Instance.ActivePatient = patient;
            PatientViewViewModel.Instance.ActivePatient = patient;
            if (patient.GetSessions().Any())
            {
                PatientViewViewModel.Instance.SelectedSession = patient.GetSessions()?.First();
            }

            InitializeComponent();

            chart.UpdateSeriesPatientBasis(patient);
            nte.rtb.IsReadOnly = true;
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
            btnStart.IsEnabled = false;
            btnEnd.IsEnabled = true;
            nte.rtb.Document.Blocks.Clear();
            nte.rtb.IsReadOnly = false;
            PatientViewViewModel.Instance.StartNewSession();
        }
        
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnEnd.IsEnabled = false;
            nte.rtb.IsReadOnly = true;
            foreach (var documentBlock in nte.rtb.Document.Blocks)
            {
                var text = new TextRange(documentBlock.ContentStart, documentBlock.ContentEnd);
                PatientViewViewModel.Instance.SendNoteToWorkspace(text.Text);
            }
            
            PatientViewViewModel.Instance.EndCurrentSession();
        }

        private void SessionsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox box))
            {
                return;
            }

            if (e.AddedItems.Count == 0)
            {
                return;
            }
            
            PatientViewViewModel.Instance.SelectedSession = (TherapySession)e.AddedItems[0];
            NotesList.ItemsSource = PatientViewViewModel.Instance.SelectedSession.GetNotesList();
        }
    }
}
