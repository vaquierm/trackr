﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace trackr.core
{
    public class Workspace
    {
        private readonly TrackrSettings _settings;

        private readonly List<TherapyPatient> _patients;

        public TherapyPatient ActivePatient { get; set; }
        
        public Workspace()
        {
            _settings = TrackrSettings.Load();
            _patients = new List<TherapyPatient>();
            Init();                     
        }
        
        private void Init()
        {
            SetupFileSystem();
            LoadPatients();
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
                if (!Directory.Exists(_settings.WorkingDirectory))
                {
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
            var workspacePath = _settings.UseDefaultDir ? _settings.DefaultWorkingDir : _settings.WorkingDirectory;
            var filename = Path.Combine(workspacePath, ActivePatient.PatientStringId + ".trck");
            if (openDialog)
            {
                var dialog = new SaveFileDialog {Filter = "Trackr file|*.trck", Title = "Save Trackr File", DefaultExt = ".trck"};
                dialog.ShowDialog();
                if (dialog.FileName != string.Empty)
                {
                    filename = dialog.FileName;
                }
            }
            File.WriteAllText(filename, JsonConvert.SerializeObject(ActivePatient));
        }

        public TherapySession StartNewSession()
        {
            return ActivePatient.NewSession();
        }

        public void EndCurrentSession()
        {
            var currentSession = ActivePatient.GetActiveSession();
            currentSession?.EndSession();
            SaveActivePatient(false);
        }
        
    }
}