using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for NoteTextEditor.xaml
    /// </summary>
    public partial class NoteTextEditor : UserControl
    {
        private const int TimeInterval = 3;
        private DateTime _lastChange;
        private readonly PatientViewViewModel _viewModel;
        
        public NoteTextEditor()
        {
            _lastChange = DateTime.MinValue;
            DataContext = PatientViewViewModel.Instance;
            _viewModel = PatientViewViewModel.Instance;
            InitializeComponent();
        }

        private void Rtb_OnKeyDown_(object sender, KeyEventArgs e)
        {
            if (!(sender is RichTextBox richTextBox))
            {
                return;
            }

            if (richTextBox.IsReadOnly)
            {
                return;
            }

            var deltaTime = DateTime.Now.Subtract(_lastChange);
            if (deltaTime.Seconds > TimeInterval)
            {
                richTextBox.AppendText("\n[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] ");
                richTextBox.CaretPosition = richTextBox.Document.Blocks.LastBlock.ContentEnd;
            }
            _lastChange = DateTime.Now;
        }
    }
}
