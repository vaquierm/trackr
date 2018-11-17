using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trackr.core;

namespace trackr.ui
{
    public class PatientViewViewModel : ViewModelBase
    {
        private static readonly Lazy<PatientViewViewModel> Lazy =
        new Lazy<PatientViewViewModel>(() => new PatientViewViewModel());
        
        public static PatientViewViewModel Instance => Lazy.Value;
        private PatientViewViewModel() {}
        
        public event EventHandler Closing;

        public System.Collections.ObjectModel.ObservableCollection<List<string>> strings { get; set; }

        #region Properties

        public TherapyPatient ActivePatient
        {
            get => Workspace.Instance.ActivePatient;
            set => Workspace.Instance.ActivePatient = value;
        }

        public TherapySession ActiveSession
        {
            get => Workspace.Instance.ActivePatient.GetActiveSession();
        }
        
        #endregion

        public void SendNoteToWorkspace(string rawNote)
        {

        }

        public void Close()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }
        
        
        
    }
}
