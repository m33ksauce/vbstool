using System;
using System.Collections.Generic;

namespace VBSTool
{
    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ParentName { get; set; }

        public Student(string First, string Last, string Parent) {
            this.FirstName = First;
            this.LastName = Last;
            this.ParentName = Parent;
        }

        public static Student ParseFromLine(string[] line)
        {
            string first = line[Config.FirstNameCol];
            string last = line[Config.LastNameCol];
            string parent = $"{line[Config.ParentFirstNameCol]} {line[Config.ParentLastNameCol]}";
            return new Student(first, last, parent);
        }

        public string PrintName()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }
}