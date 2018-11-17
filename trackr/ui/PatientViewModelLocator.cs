namespace trackr.ui
{
    public class PatientViewModelLocator
    {
        public static PatientViewViewModel ViewModel
        {
            get => ViewModel ?? (ViewModel = new PatientViewViewModel());
            private set => ViewModel = value;
        }
    }
}