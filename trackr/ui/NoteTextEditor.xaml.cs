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
        private readonly Timer _saveTimer;
        
        public NoteTextEditor()
        {
            _lastChange = DateTime.MinValue;
            DataContext = PatientViewViewModel.Instance;
            _viewModel = PatientViewViewModel.Instance;
            InitializeComponent();
            _saveTimer = new Timer(5000) {AutoReset = true};
            _saveTimer.Elapsed += OnTimedEvent;
            _saveTimer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _saveTimer.Stop();
            var text = new TextRange(rtb.Document.Blocks.LastBlock.ContentStart, rtb.Document.Blocks.LastBlock.ContentEnd);
            _viewModel.SendNoteToWorkspace(text.Text);
            _saveTimer.Start();
        }

        private void Rtb_OnKeyDown_(object sender, KeyEventArgs e)
        {
            if (!(sender is RichTextBox richTextBox))
            {
                return;
            }

            var deltaTime = DateTime.Now.Subtract(_lastChange);
            if (deltaTime.Seconds > TimeInterval)
            {
                // TODO: Send note to viewModel to save it
                var text = new TextRange(richTextBox.Document.Blocks.LastBlock.ContentStart, richTextBox.Document.Blocks.LastBlock.ContentEnd);
                _viewModel.SendNoteToWorkspace(text.Text);
                
                richTextBox.AppendText("\n[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] ");
                richTextBox.CaretPosition = richTextBox.Document.Blocks.LastBlock.ContentEnd;
            }
            _lastChange = DateTime.Now;

        }
    }
}
