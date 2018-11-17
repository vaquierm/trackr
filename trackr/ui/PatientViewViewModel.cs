using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trackr.ui
{
    class PatientViewViewModel : ViewModelBase
    {
        public event EventHandler Closing;

        public System.Collections.ObjectModel.ObservableCollection<List<string>> strings { get; set; }

        public void Close()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }
    }
}
