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


        public void Close()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }
        
        
        
    }
}
