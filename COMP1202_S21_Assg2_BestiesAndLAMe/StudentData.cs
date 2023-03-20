using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace COMP1202_S21_Assg2_BestiesAndLAMe
{
    class StudentData
    {
        public static int IDGenerator = 10000;
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }


        public StudentData()
        {
            StudentID = GenerateStudentID();
            FirstName = "";
            LastName = "";
            Major = "";
            GPA = 0;
            Phone = "";
            DateOfBirth = "";
        }
        public StudentData(int studentID, string fName, string lName, string major, double gpa, string phone, string dateOfBirth)
        {
            StudentID = studentID;
            FirstName = fName;
            LastName = lName;
            Major = major;
            GPA = gpa;
            Phone = phone;
            DateOfBirth = dateOfBirth;
        }
        public StudentData(string fName, string lName, string major, double gpa, string phone, string dateOfBirth)
        {
            StudentID = GenerateStudentID();
            FirstName = fName;
            LastName = lName;
            GPA = gpa;
            Major = major;
            Phone = phone;
            DateOfBirth = dateOfBirth;
        }
        public override string ToString()
        {
            string str = "";
            str += String.Format("{0,-15}:  {1,-15}", "Student ID", StudentID) + String.Format("\n{0,-15}:  {1,-15}", "First Name", FirstName) +
                String.Format("\n{0,-15}:  {1,-15}", "Last Name", LastName) + String.Format("\n{0,-15}:  {1,-15}", "Date of Birth", DateOfBirth) + String.Format("\n{0,-15}:  {1,-15}", "Major", Major) +
                String.Format("\n{0,-15}:  {1,-15}", "GPA", GPA) + String.Format("\n{0,-15}:  {1,-15}", "Phone Number", Phone);
            return str;
        }

        // given a ToString'd Student return a new student object
        public StudentData(string str)
        {
            string[] data = str.Split(':');
            this.StudentID = Convert.ToInt32(data[0]);
            this.FirstName = data[1];
            this.LastName = data[2];
            this.Major = data[3];
            this.GPA = Convert.ToDouble(data[4]);
            this.Phone = data[5];
            this.DateOfBirth = data[6];
        }
        public string ToJson()
        {
            return StudentID + ":" + FirstName + ":" + LastName + ":" + Major + ":" + GPA + ":" + Phone + ":" + DateOfBirth;
        }
        private int GenerateStudentID()
        {
            IDGenerator++;
            // Access the file and allow data to be appended
            StreamWriter write = new StreamWriter("ListOfID.txt", true);
            // Append generated ID to the file
            write.WriteLine(IDGenerator);
            write.Close();

            return IDGenerator - 1;
        }
    }
}