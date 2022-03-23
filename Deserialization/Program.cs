using System.Text.Json;
using Deserialization;


class Program
{
    public const string FILEPATH = "students.json";
    public const int TUITUIONFEE = 10000;
    private static List<Student> students;
    public static void Main()
    {
        bool endProgram = false;
        string userInput = "";
        students = Deserialize();

        while (!endProgram)
        {
            Console.WriteLine(@"
            Choose an action to perform:
            A - Add student
            R - Remove student
            P - Print students list
            TF - Print students tuition fee report
            Q - Quit");
            userInput = Console.ReadLine().ToLower().Trim();
            switch (userInput)
            {
                case "a":
                    AddStudent();
                    break;
                case "r":
                    Console.WriteLine("Enter student's name to remove:");
                    RemoveStudent(Console.ReadLine());
                    break;
                case "p":
                    PrintStudents();
                    break;
                case "tf":
                    PrintStudentsTuitionFeeReport();
                    break;
                case "q":
                    endProgram = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }
    public static void AddStudent()
    {
        string name = "";
        int age = 0;
        // Input
        while (name == "")
        {
            Console.WriteLine("Enter student Name:");
            name = Console.ReadLine().Trim();
            if (name == "") Console.WriteLine("Name field have to include at least 1 char, Please try again.");
        }

        while (age == 0)
        {
            Console.WriteLine("Enter student Age:");
            try
            {
                age = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            { Console.WriteLine("Age field accepts only digits, please try again."); }
        }
        Student addStudent = new Student(name, age);

        // Add process
        foreach (Student student in students)
        {
            if (addStudent.Name.ToLower().Trim() == student.Name.ToLower())
            {
                Console.WriteLine($"Cannot add {addStudent}, student named {addStudent.Name} already exist!");
                return;
            }
        }
        students.Add(addStudent);
        Console.WriteLine($"{addStudent} added successfully.");

        SerializeAndUpdate(students);
    }
    public static void RemoveStudent(string studentName)
    {
        bool removed = false;
        foreach (Student student in students)
        {
            if (student.Name.ToLower().Trim() == studentName.ToLower())
            {
                removed = students.Remove(student);
                SerializeAndUpdate(students);
                break;
            }
        }
        if (removed)
            Console.WriteLine($"{studentName} removed successfully.");
        else
            Console.WriteLine($"Cannot remove {studentName}, student isn't exist!");

    }
    public static void PrintStudents()
    {
        for (int i = 0; i < students.Count; i++)
            Console.WriteLine($"Student Number [{i + 1}] : {students[i]}");
    }

    public static void PrintStudentsTuitionFeeReport()
    {
        var checkAge = (int age) => { if (age >= 25) return (TUITUIONFEE.ToString()); else return ((TUITUIONFEE / 10 * 9).ToString()); };
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{students[i]} need to pay {checkAge(students[i].Age)}");
        }
    }
    public static List<Student> Deserialize()
    {
        string jsonString = File.ReadAllText(FILEPATH);
        List<Student> students = JsonSerializer.Deserialize<List<Student>>(jsonString);
        return students;
    }
    public static void SerializeAndUpdate(List<Student> students)
    {
        string jasonStr = JsonSerializer.Serialize(students);
        File.WriteAllText(FILEPATH, jasonStr);
    }

}