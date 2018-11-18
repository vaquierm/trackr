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

        public string ActivePatientNextDate
        {
            get
            {
                try
                {
                    return Workspace.Instance.ActivePatient?.NextSession.ToShortDateString();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public TherapyPatient ActivePatient
        {
            get => Workspace.Instance.ActivePatient;
            set => Workspace.Instance.ActivePatient = value;
        }

        public TherapySession ActiveSession => Workspace.Instance.ActivePatient.GetActiveSession();

        public bool SessionStarted { get; set; }

        public List<TherapySession> ActivePatientSessions => ActivePatient?.GetSessions();

        private TherapySession _selectedSession;
        public TherapySession SelectedSession
        {
            get => _selectedSession; 
            set
            {
                _selectedSession = value;
                SelectedSessionNotes = value?.GetNotesList();
            }
        }

        public List<Note> SelectedSessionNotes { get; set; }

        #endregion

        public void SendNoteToWorkspace(string rawNote)
        {
            var tokens = rawNote.Split(new[] {'[', ']'}, 2, StringSplitOptions.RemoveEmptyEntries);
            if (!tokens.Any()) return;
            try
            {
                var timeStamp = DateTime.ParseExact(tokens[0], "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                var noteContent = tokens[1];
                ActivePatient.GetActiveSession().InsertNote(timeStamp, noteContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StartNewSession()
        {
            Workspace.Instance.EndCurrentSession();
            Workspace.Instance.StartNewSession();
            SessionStarted = true;
        }

        public void EndCurrentSession()
        {
            Workspace.Instance.EndCurrentSession();
            SessionStarted = false;
        }

        public void Close()
        {
            if (ActiveSession.SessionRunning)
            {
                ActivePatient.EndSession();
            }
            Closing?.Invoke(this, EventArgs.Empty);
        }
        
        
        
    }
}
