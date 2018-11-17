using System;

namespace trackr.core
{
    public struct TherapyClientInfo
    {
        public enum Gender { Male, Female, Other }
        
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Sex { get; set; }
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