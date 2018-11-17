using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using trackr.imgProcessing;
using trackr.ui;

namespace trackr.core
{
    public sealed class Workspace
    {
        private static readonly Lazy<Workspace> Lazy =
        new Lazy<Workspace>(() => new Workspace());
        
        public static Workspace Instance => Lazy.Value;
        private Workspace() {}

        private TrackrSettings _settings;

        private List<TherapyPatient> _patients;

        public TherapyPatient ActivePatient { get; set; }
        
        public void Init()
        {
            _settings = TrackrSettings.Load();
            _patients = new List<TherapyPatient>();
            SetupFileSystem();
            LoadPatients();
            
            // TEST PURPOSES
            //PatientViewViewModel.Instance.ActivePatient = _patients.First();
            //PatientViewViewModel.Instance.ActivePatient.NewSession();
            // TEST PURPOSES
            
        }

        private void CameraControllerOnNewEmotionDataAvailable(object sender, EventArgs e)
        {
            if (!(e is CameraController.NewEmotionDataEventArgs args))
            {
                return;
            }

            var data = args.Data;
            ActivePatient.GetActiveSession()?.InsertEmotionData(data);
        }

        public void Close()
        {
            EndCurrentSession();
            foreach (var patient in _patients)
            {
                foreach (var therapySession in patient.GetSessions())
                {
                    if (therapySession.SessionRunning) therapySession.EndSession();
                }
                SavePatient(patient, false);
            }
            _settings.Save();
        }

        private void SetupFileSystem()
        {
            if (_settings.UseDefaultDir)
            {
                if (!Directory.Exists(_settings.DefaultWorkingDir))
                {
                    Directory.CreateDirectory(_settings.DefaultWorkingDir);
                }
            }
            else
            {
                if (Directory.Exists(_settings.WorkingDirectory)) return;
                var result = MessageBox.Show("The workspace directory does not exist, Do you wish to create it?\n" +
                                             "Choosing \"no\" will revert to the default working directory",
                    "Warning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Directory.CreateDirectory(_settings.WorkingDirectory);
                        break;
                    case MessageBoxResult.No:
                        _settings.UseDefaultDir = true;
                        _settings.Save();
                        break;
                }
            }
        }

        private void LoadPatients()
        {
            var workspacePath = _settings.UseDefaultDir ? _settings.DefaultWorkingDir : _settings.WorkingDirectory;
            var files = Directory.GetFiles(workspacePath, "*.trck", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                _patients.Add(JsonConvert.DeserializeObject<TherapyPatient>(File.ReadAllText(file)));
            }
        }

        public List<TherapyPatient> GetPatients()
        {
            return _patients;
        }
        
        public TherapyPatient GetTherapyPatientFromStringId(string id)
        {
            return _patients.Find(x => x.PatientStringId == id);
        }

        public List<TherapySession> GetTherapySessionsFromStringId(string id)
        {
            return _patients.Find(x => x.PatientStringId == id).GetSessions();
        }

        public void AddNewPatient(TherapyPatient newPatient, bool setActive = true)
        {
            _patients.Add(newPatient);
            if (setActive)
            {
                ActivePatient = newPatient;
            }
        }

        public void SaveActivePatient(bool openDialog)
        {
            SavePatient(ActivePatient, openDialog);
        }

        private void SavePatient(TherapyPatient patient, bool openDialog)
        {
            var workspacePath = _settings.UseDefaultDir ? _settings.DefaultWorkingDir : _settings.WorkingDirectory;
            var filename = Path.Combine(workspacePath, patient.PatientStringId + ".trck");
            File.WriteAllText(filename, JsonConvert.SerializeObject(patient, Formatting.Indented));
        }

        public TherapySession StartNewSession()
        {
            CameraController.NewEmotionDataAvailable += CameraControllerOnNewEmotionDataAvailable;
            CameraController.StartCapture();
            return ActivePatient.NewSession();
        }

        
        public void EndCurrentSession()
        {
            if (ActivePatient?.GetActiveSession() == null)
                return;

            if (!ActivePatient.GetSessions().Any())
            {
                return;
            }
            if (ActivePatient.GetSessions().Any() && !ActivePatient.GetActiveSession().SessionRunning)
            {
                return;
            }
            CameraController.NewEmotionDataAvailable -= CameraControllerOnNewEmotionDataAvailable;
            CameraController.StopCapture();
            ActivePatient.EndSession();
            SaveActivePatient(false);
        }
    }
}
