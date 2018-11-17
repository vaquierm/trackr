using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for NoteTextEditor.xaml
    /// </summary>
    public partial class NoteTextEditor : UserControl
    {
        private const int TimeInterval = 5;
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
            if (!(sender is RichTextBox rtb))
            {
                return;
            }
           
            // Make sure key is AlphaNumeric before creating a new note and moving the cursor
            var keyValue = (int) e.Key;
            if (!(keyValue >= 0x30 && keyValue <= 0x39 // numbers
             || keyValue >= 0x41 && keyValue <= 0x5A // letters
             || keyValue >= 0x60 && keyValue <= 0x69)) // numpad
            {
                return;
            }

            var deltaTime = DateTime.Now.Subtract(_lastChange);
            if (deltaTime.Seconds > TimeInterval)
            {
                
                // TODO: Send note to viewModel to save it
                _viewModel.SendNoteToWorkspace(rtb.Document.Blocks.LastBlock.ToString());
                
                rtb.AppendText("\n" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " : ");
                rtb.CaretPosition = rtb.Document.Blocks.LastBlock.ContentEnd;
            }
            _lastChange = DateTime.Now;
            

            // Start new block and place caret
            // rtb.AppendText(e.Key.ToString());
        }
    }
}
