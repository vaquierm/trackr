using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using trackr.core;

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for NewPatientForm.xaml
    /// </summary>
    public partial class NewPatientForm : Window
    {
        //The new patient created
        public TherapyPatient newPatient = null;

        //The home page
        TrackrHome homePage;

        public NewPatientForm(TrackrHome home)
        {
            InitializeComponent();

            genderComboBox.Items.Add("Female");
            genderComboBox.Items.Add("Male");
            genderComboBox.Items.Add("Other");

            homePage = home;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            var date = birthDayDatePicker.SelectedDate;

            if (firstNameTextBox.Text == string.Empty || lastNameTextBox.Text == string.Empty)
            {
                errorMessageTextBlock.Text = "The name field cannot be empty";
            }

            if (date == null)
            {
                errorMessageTextBlock.Text = "A valid date must be selected.";
                return;
            }

            if (genderComboBox.SelectedItem is string && genderComboBox.SelectedItem == null)
            {
                errorMessageTextBlock.Text = "A gender must be selected";
                return;
                
            }

            newPatient = new TherapyPatient(firstNameTextBox.Text, lastNameTextBox.Text, genderToString(genderComboBox.SelectedItem as string), (DateTime)birthDayDatePicker.SelectedDate);

            Workspace.Instance.AddNewPatient(newPatient);

            homePage?.RefreshPatientButtons();

            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private Gender genderToString(string gender)
        {
            Gender g;
            switch(gender)
            {
                case "Female":
                    g = Gender.Female;
                    break;
                case "Male":
                    g = Gender.Male;
                    break;
                default:
                    g = Gender.Other;
                    break;
            }
            return g;
        }


    }
}
