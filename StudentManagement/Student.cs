using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class Student
    {
        // Properties (start with capital)

        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string? City { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public GenderType Gender { get; set; }

        // calculated properties

        // The Fullname property is one expression so a shorter
        // syntax with Lambda expression can be used
        public string Fullname => $"{Firstname} {Lastname}";

        public int Age
        {
            get
            {
                // 2022 - 1974 = 48
                int age = DateTime.Now.Year - DateOfBirth.Year;


                // 7-2-2022
                // Check if your birthday is later this year
                
                if (DateOfBirth.AddYears(age) > DateTime.Now)
                    age--;

                return age;
            }
        }

        public int AgeInDays
        {
            get
            {
                TimeSpan difference = DateTime.Now - DateOfBirth;

                return difference.Days;
            }
        }
    }

    enum GenderType
    {
        Unknown, Male, Female
    }
}
