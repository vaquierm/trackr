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

        #region Properties

        public string ActivePatientLastDate
        {
            get
            {
                try
                {
                    return Workspace.Instance.ActivePatient?.GetSessions().Last().StartDateTime.ToShortDateString();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public string ActivePatientName
        {
            get => Workspace.Instance.ActivePatient?.Info.Name + " " + Workspace.Instance.ActivePatient?.Info.LastName;
        }

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
