using System;

namespace trackr.core
{
    public enum Gender { Male, Female, Other }
    
    public struct TherapyPatientInfo
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public int Age()
        {
            var now = DateTime.Today;
            var age = now.Year - BirthDate.Year;
            if (now < BirthDate.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}