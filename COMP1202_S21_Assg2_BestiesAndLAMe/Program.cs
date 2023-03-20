// COMP 1202
// BestiesAndLAMe
// Ashley Quyen Ly-Do Student ID: 101353571
// Elizaveta Vygovskaia Student ID: 101337015
// Huy Lam Student ID: 101069146

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

namespace COMP1202_S21_Assg2_BestiesAndLAMe
{
    class Program
    {
        static void Main(string[] args)
        {
            ForegroundColor = ConsoleColor.Yellow;
            CentersText("Welcome!");
            CentersText("This program let you input/view/modify student information.");
            CentersText("To continue press any key. To exit the program press \"E\".\n");

            Exit(ConsoleKey.E);
            ResetColor();
            OpenFile();

            Clear();
            DisplayMenu();
            ReadLine();
        }
        public static void Exit(ConsoleKey key)
        {
            //exit the program if user pressed key button
            ConsoleKeyInfo getKey = Console.ReadKey(true);
            if (getKey.Key == key)
            {
                Environment.Exit(-1);
            }
        }
        public static void CentersText(String text)
        {
            //centers text in the console 
            Write(new string(' ', (WindowWidth - text.Length) / 2));
            WriteLine(text);
        }
        public static void DisplayMenu()
        {
            WriteLine("Please select your option: \n");
            switch (GetUserChoice())
            {
                case 1:
                    AddNewStudent();
                    break;

                case 2:
                    ModifyStudentData();
                    break;
                case 3:
                    DisplayData();
                    break;
                case 4:
                    SearchForStudent();
                    break;
                default:
                    break;
            }
        }
        static int GetUserChoice()
        {
            // Adds new option to the menu
            var newOption = new Dictionary<int, string>();
            newOption[1] = "Add new student.";
            newOption[2] = "Modify student's data.";
            newOption[3] = "Display all students.";
            newOption[4] = "Search for a student.";
            while (true)
            {
                foreach (var option in newOption)
                    WriteLine("{0}. {1}", option.Key, option.Value);
                Write("\nYour choice: ");
                var userChoice = ReadLine();
                var selected = newOption.SingleOrDefault(x => x.Key.ToString() == userChoice);

                if (default(KeyValuePair<int, string>).Equals(selected))
                {

                    ErrorMessage("\nPlease use only digits from 1 to 4\n");
                    continue;
                }
                Clear();
                return selected.Key;
            }
        }
        public static void AddNewStudent()
        {
            Write("First Name: ");
            string firstName = ValidateName(ReadLine());

            Write("Last Name: ");
            string lastName = ValidateName(ReadLine());

            Write("Major: ");
            string major = ValidateMajor(ReadLine());

            Write("GPA: ");
            string gpa = ValidateGPA(ReadLine());

            Write("Phone Number (###-###-####): ");
            string phone = ValidatePhone(ReadLine());

            Write("Date of birth (dd/mm/yyyy, dd-mm-yyyy or dd.mm.yyyy): ");
            string date = ValidateDateOfBirth(ReadLine());

            StudentData student = new StudentData(firstName, lastName, major, Convert.ToDouble(gpa), phone, date);
            StreamWriter write = new StreamWriter("StudentData.txt", true);
            write.WriteLine(student.ToJson());
            write.Close();
            ForegroundColor = ConsoleColor.DarkGreen;
            Write("\nYou have successfully added {0} {1}'s data to the file.\n\n", firstName, lastName);
            ResetColor();
            ExitOrContinue();
        }
        public static void ModifyStudentData()
        {

            Clear();
            string message = "\nYou have successfully updated student's data!\n";
            Write("Please enter a Student ID: ");
            string studentID = ValidateStudentID(ReadLine());
            string line;
            DarkYellowColor("Student's data by ID: " + studentID + "\n");
            StreamReader read = new StreamReader("StudentData.txt");
            bool found = false;
            while ((line = read.ReadLine()) != null)
            {
                
                string[] studentData = line.Split(':');
                if (studentID == line.Split(':')[0])
                {
                    Clear();
                    StudentData studentDisplay = new StudentData(line);
                    WriteLine(studentDisplay.ToString());
                    WriteLine("*******************************************\n");
                    found = true;
                    break;
                   
                }
            }

            if(!found){
                NoDataFound();
                ExitOrContinue();
                DisplayMenu();
            }

            read.Close();
            int lineNum = Convert.ToInt32(studentID) - 9999;
            if (lineNum <= 0)
            {
                NoDataFound();
            }
            using (StreamReader inputFile = new StreamReader("StudentData.txt"))
            {
                //Skipping all lines we are not interested in
                for (int i = 1; i < lineNum; i++)
                {
                    inputFile.ReadLine();
                }
                //Specific line we want to read
                line = inputFile.ReadLine();
                inputFile.Close();
            }
            StudentData student = new StudentData(line);

            WriteLine("\nPlease select what would you like to modify \n\n1.First Name\n2.Last Name\n3.Major\n4.GPA\n5.Phone Number\n6.Date Of Birth ");
            Write("\nYour choice: ");

            string userChoice = ReadLine();
            Regex choice = new Regex(@"^([1-6]){1}$");
            while (!choice.IsMatch(userChoice))
            {
                ErrorMessage("\nPlease choose correct option!\n");
                Write("Your choice: ");
                userChoice = ReadLine();
            }
            switch (userChoice)
            {
                case "1":
                    Write("\nUpdate First Name: ");
                    string firstName = ValidateName(ReadLine());
                    student.FirstName = firstName;
                    string newLine = student.ToJson();
                    LineChanger(newLine, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                case "2":
                    Write("\nUpdate Last Name: ");
                    string lastName = ValidateName(ReadLine());
                    student.LastName = lastName;
                    string newLine1 = student.ToJson();
                    LineChanger(newLine1, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                case "3":
                    Write("\nUpdate Major: ");
                    string major = ValidateMajor(ReadLine());
                    student.Major = major;
                    string newLine2 = student.ToJson();
                    LineChanger(newLine2, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                case "4":
                    Write("\nUpdate GPA: ");
                    string gpa = ValidateGPA(ReadLine());
                    student.GPA = Convert.ToDouble(gpa);
                    string newLine3 = student.ToJson();
                    LineChanger(newLine3, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                case "5":
                    Write("\nUpdate Phone Number (###-###-####): ");
                    string phone = ValidatePhone(ReadLine());
                    student.Phone = phone;
                    string newLine4 = student.ToJson();
                    LineChanger(newLine4, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                case "6":
                    Write("\n Update Date of birth (dd/mm/yyyy, dd-mm-yyyy or dd.mm.yyyy): ");
                    string date = ValidateDateOfBirth(ReadLine());
                    student.DateOfBirth = date;
                    string newLine5 = student.ToJson();
                    LineChanger(newLine5, "StudentData.txt", lineNum);
                    DarkGreenColor(message);
                    ExitOrContinue();
                    break;
                default:
                    break;
            }
        }
        public static void DisplayData()
        {
            Regex choice = new Regex(@"^([1-4]){1}$");
            WriteLine("Please select sorting order \n\n1. Default\n2. By First Name\n3. By Last Name\n4. By GPA");
            Write("\nYour choice: ");
            string userChoice = ReadLine();
            StreamReader read = new StreamReader("StudentData.txt");
            while (!choice.IsMatch(userChoice))
            {
                ErrorMessage("\nPlease choose correct option!\n");
                Write("Your choice: ");
                userChoice = ReadLine();
            }
            if (userChoice == "1")
            {
                Clear();
                string line;
                DarkYellowColor("Displaying student's data by default:\n");
                int countLines = 0;
                while ((line = read.ReadLine()) != null)
                {
                    StudentData studentDisplay = new StudentData(line);
                    WriteLine(studentDisplay.ToString());
                    WriteLine("\n---------------------------------------\n");
                    countLines++;
                }
                if (countLines == 0)
                {
                    NoDataFound();
                }
                read.Close();
                ExitOrContinue();
                DisplayMenu();
            }

            if (userChoice == "2")
            {
                Clear();
                WriteLine("Please select one option \n\n1.ASCENDING(A - Z)\n2.DESCENDING(Z - A)\n");
                Write("Your choice: ");
                string userSubChoice = ReadLine();
                ValidateInput(userSubChoice);
                Clear();
                switch (userSubChoice)
                {
                    case "1":
                        DarkYellowColor("Displaying student's data by First Name (A-Z):\n");
                        SortStudentBy('F', userSubChoice);
                        break;
                    case "2":
                        DarkYellowColor("Displaying student's data by First Name (Z-A):\n");
                        SortStudentBy('F', userSubChoice);
                        break;
                    default:
                        break;
                }

                ExitOrContinue();
                DisplayMenu();
            }

            if (userChoice == "3")
            {
                Clear();
                WriteLine("Please select one option \n\n1.ASCENDING(A - Z)\n2.DESCENDING(Z - A)\n");
                Write("Your choice: ");
                string userSubChoice = ReadLine();
                ValidateInput(userSubChoice);
                Clear();
                switch (userSubChoice)
                {
                    case "1":
                        DarkYellowColor("Displaying student's data by Last Name (A-Z):\n");
                        SortStudentBy('L', userSubChoice);
                        break;
                    case "2":
                        DarkYellowColor("Displaying student's data by Last Name (Z-A):\n");
                        SortStudentBy('L', userSubChoice);
                        break;
                    default:
                        break;
                }
                ExitOrContinue();
                DisplayMenu();
            }
            if (userChoice == "4")
            {
                Clear();
                WriteLine("Please select one option \n\n1.ASCENDING(Lowest to Highest)\n2.DESCENDING(Highest to Lowest)\n");
                Write("Your choice: ");
                string userSubChoice = ReadLine();
                ValidateInput(userSubChoice);
                Clear();
                switch (userSubChoice)
                {
                    case "1":
                        DarkYellowColor("Displaying student's data by GPA (0-4):\n");
                        SortStudentBy('1', userSubChoice);
                        break;
                    case "2":
                        DarkYellowColor("Displaying student's data by GPA (4-0):\n");
                        SortStudentBy('1', userSubChoice);
                        break;
                    default:
                        break;
                }
                ExitOrContinue();
                DisplayMenu();
            }

        }
        public static void SearchForStudent()
        {
            Regex choice = new Regex(@"^([1-3])$");
            StreamReader read = new StreamReader("StudentData.txt");
            WriteLine("Please select search by \n\n1. By Student ID\n2. By Major\n3. By GPA");
            WriteLine("\nYour choice: ");
            string userChoice = ReadLine();
            while (!choice.IsMatch(userChoice))
            {
                ErrorMessage("\nPlease choose correct option!\n");
                Write("Your choice: ");
                userChoice = ReadLine();
            }
            switch (userChoice)
            {
                case "1":
                    Clear();
                    Write("Please enter a Student ID: ");
                    string studentID = ValidateStudentID(ReadLine());
                    string line;
                    DarkYellowColor("Student's data by ID: " + studentID + "\n");
                    while ((line = read.ReadLine()) != null)
                    {
                        int conutLinesId = 0;
                        string[] studentData = line.Split(':');
                        if (studentID == studentData[0])
                        {
                            Clear();
                            StudentData studentDisplay = new StudentData(line);
                            WriteLine(studentDisplay.ToString());
                            WriteLine("*******************************************\n");
                            conutLinesId++;
                        }
                        if (conutLinesId == 0)
                        {
                            NoDataFound();
                        }
                    }
                    ExitOrContinue();
                    DisplayMenu();
                    break;
                case "2":
                    Clear();
                    Write("Please enter major: ");
                    string major = ValidateMajor(ReadLine());
                    DarkYellowColor("Student's data by major: " + major + "\n");
                    using (StreamReader str = new StreamReader("StudentData.txt"))
                    {
                        int conutLinesMajor = 0;
                        while (!str.EndOfStream)
                        {
                            line = str.ReadLine();
                            if (!String.IsNullOrEmpty(line))
                            {
                                if (line.IndexOf(major, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    WriteLine();
                                    StudentData student = new StudentData(line);
                                    WriteLine(student.ToString());
                                    WriteLine("\n*******************************************\n");
                                    conutLinesMajor++;
                                }
                            }
                        }
                        if (conutLinesMajor == 0)
                        {
                            NoDataFound();
                        }
                    }
                    read.Close();
                    ExitOrContinue();
                    DisplayMenu();
                    break;
                case "3":
                    Clear();
                    Write("Please enter GPA: ");
                    string gpa = ValidateGPA(ReadLine());
                    Clear();
                    WriteLine("Please select one option \n\n1.LOWER VALUES\n2.HIGHER VALUES\n");
                    Write("Your choice: ");
                    string userSubChoice = ValidateInput(ReadLine());
                    Clear();

                    string line1;
                    if (userSubChoice == "1")
                    {
                        using (StreamReader readData = new StreamReader("StudentData.txt"))
                        {
                            DarkYellowColor("GPA lower than: " + gpa + "\n");
                            int conutLinesGPA = 0;
                            while ((line1 = readData.ReadLine()) != null)
                            {

                                string[] studentData = line1.Split(':');
                                if (Convert.ToDouble(studentData[4]) < Convert.ToDouble(gpa))
                                {
                                    StudentData student = new StudentData(line1);
                                    WriteLine(student.ToString());
                                    WriteLine("\n*******************************************\n");
                                    conutLinesGPA++;
                                }
                            }
                            if (conutLinesGPA == 0)
                            {
                                NoDataFound();
                            }
                        }
                        ExitOrContinue();
                        DisplayMenu();
                    }
                    if (userSubChoice == "2")
                    {
                        using (StreamReader readData = new StreamReader("StudentData.txt"))
                        {
                            DarkYellowColor("GPA higher than: " + gpa + "\n");
                            int conutLinesGpa = 0;
                            while ((line1 = readData.ReadLine()) != null)
                            {
                                string[] studentData = line1.Split(':');
                                if (Convert.ToDouble(studentData[4]) > Convert.ToDouble(gpa))
                                {
                                    StudentData student = new StudentData(line1);
                                    WriteLine(student.ToString());
                                    WriteLine("\n*******************************************\n");
                                    conutLinesGpa++;
                                }
                            }
                            if (conutLinesGpa == 0)
                            {
                                NoDataFound();
                            }
                        }
                        ExitOrContinue();
                        DisplayMenu();
                    }
                    read.Close();
                    ExitOrContinue();
                    DisplayMenu();
                    break;
            }
        }
        public static string ValidateStudentID(string studentID)
        {
            Regex id = new Regex(@"^1\d{4,5}$");
            while (!id.IsMatch(studentID))
            {
                ErrorMessage("\nIncorrect Student ID!\n");
                Write("Please enter a Student ID: ");
                studentID = ReadLine();
            }
            return studentID;
        }
        public static string ValidateName(string name)
        {
            Regex namePattern = new Regex(@"^[a-zA-Z]+$");
            while (!namePattern.IsMatch(name)) // the first name is valid
            {
                //if name is not valid => contains white space, digits, empty or null or special characters 
                ErrorMessage("\nPlease use only letters!\n");
                Write("Enter valid name: ");
                name = ReadLine();
            }
            return name;
        }
        public static string ValidateMajor(string major)
        {
            Regex majorPattern = new Regex(@"^[A-Za-z][A-Za-z\s]*[A-Za-z]|[A-Za-z]$");
            while (!majorPattern.IsMatch(major)) // the first name is valid
            {
                //if major is not valid => contains  digits, empty or null or special characters 
                ErrorMessage("\nPlease use only letters!\n");
                Write("Major: ");
                major = ReadLine();
            }
            return major;
        }
        public static string ValidateGPA(string gpa)
        {
            Regex gpaPattern = new Regex(@"(^[0-4]$)|(^[0-3](\.\d{1,2})?$)|(^[4]\.[0][0]$)");
            while (!gpaPattern.IsMatch(gpa)) // the first name is valid
            {
                //if GPA is not valid => contains  letters, empty, null, special characters or more than 4
                ErrorMessage("\nPlease enter digits only (decimal format allowed)!\n");
                Write("GPA: ");
                gpa = ReadLine();
            }
            return gpa;
        }
        public static string ValidatePhone(string phone)
        {
            Regex phonePattern = new Regex(@"^\d\d\d-\d\d\d-\d\d\d\d$");
            while (!phonePattern.IsMatch(phone)) // the first name is valid
            {
                //if phone is not valid => contains  letters, empty, null, special characters or in incorrect format
                ErrorMessage("\nPlease use only digits in correct format!\n");
                Write("Phone Number (###-###-####): ");
                phone = ReadLine();
            }
            return phone;
        }
        public static string ValidateDateOfBirth(string date)
        {
            Regex datePattern = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
            while (!datePattern.IsMatch(date)) // the first name is valid
            {
                //if date is not valid 
                ErrorMessage("\nPlease use correct format!\n");
                Write("Date of birth (dd/mm/yyyy, dd-mm-yyyy or dd.mm.yyyy): ");
                date = ReadLine();
            }
            return date;
        }
        public static string ValidateInput(string input)
        {
            Regex validPattern = new Regex(@"^([1-2])$");
            while (!validPattern.IsMatch(input)) // the first name is valid
            {
                //if date is not valid 
                ErrorMessage("\nPlease use correct format!\n");
                Write("Your choice: ");
                input = ReadLine();
            }
            return input;
        }
        public static void ErrorMessage(string message)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("\nERROR - INVALID INPUT  {0}", message);
            ResetColor();
        }

        public static void SortStudentBy(char sortBy, string sortOrder)
        {
            string line;
            List<StudentData> studentList = new List<StudentData>();
            StreamReader read = new StreamReader("StudentData.txt");
            int count = 0;
            while ((line = read.ReadLine()) != null)
            {
                string[] studentData = line.Split(':');
                studentList.Add(new StudentData(Convert.ToInt32(studentData[0]), studentData[1], studentData[2], studentData[3], Convert.ToDouble(studentData[4]), studentData[5], studentData[6]));
                count++;
            }
            read.Close();
            // 'F' => sort by first name    "A" => ascending order
            // 'L'=> sort by last name      "B" => descending order
            if (sortBy == 'F' && sortOrder == "1")
            {
                studentList.Sort((x, y) => x.FirstName.CompareTo(y.FirstName));
            }
            else if (sortBy == 'F' && sortOrder == "2")
            {
                studentList.Sort((x, y) => y.FirstName.CompareTo(x.FirstName));
            }
            else if (sortBy == 'L' && sortOrder == "1")
            {
                studentList.Sort((x, y) => x.LastName.CompareTo(y.LastName));
            }
            else if (sortBy == 'L' && sortOrder == "2")
            {
                studentList.Sort((x, y) => y.LastName.CompareTo(x.LastName));
            }
            else if (sortBy == '1' && sortOrder == "1")
            {
                studentList.Sort((x, y) => x.GPA.CompareTo(y.GPA));
            }
            else if (sortBy == '4' && sortOrder == "1")
            {
                studentList.Sort((x, y) => x.GPA.CompareTo(y.GPA));
            }
            foreach (StudentData s in studentList)
            {
                WriteLine(s.ToString());
                WriteLine("\n**************************************************\n");

            }
            if (count == 0)
            {
                NoDataFound();
            }
        }
        public static void LineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        public static void ExitOrContinue()
        {
            DarkCyanColor("Press any key to continue\nPress \"E\" to exit");
            Exit(ConsoleKey.E);
            Clear();
            DisplayMenu();
        }
        public static void OpenFile()
        {
            string path = "ListOfID.txt";
            // Every time we open the program this field should be initialized from this file
            string lastLine = "";
            // Create a file if it does not exist
            if (!File.Exists(path))
            {
                // Creates a file in bin/Debug
                File.Create(path);
            }
            else if (File.Exists(path))
            {
                StreamReader read = new StreamReader(path);
                string line;

                while ((line = read.ReadLine()) != null)
                {
                    lastLine = line;
                }
                if (lastLine != "")
                {
                    StudentData.IDGenerator = Convert.ToInt32(lastLine);
                }
                read.Close();
            }
        }
        public static void NoDataFound()
        {
            Clear();
            DarkGrayColor("\nNo data found\n");
        }
        public static void DarkGreenColor(string str)
        {
            ForegroundColor = ConsoleColor.DarkGreen;
            WriteLine(str);
            ResetColor();
        }
        public static void DarkYellowColor(string str)
        {
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine(str);
            ResetColor();
        }
        public static void DarkCyanColor(string str)
        {
            ForegroundColor = ConsoleColor.DarkCyan;
            WriteLine(str);
            ResetColor();
        }
        public static void DarkGrayColor(string str)
        {
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine(str);
            ResetColor();
        }

    }
}