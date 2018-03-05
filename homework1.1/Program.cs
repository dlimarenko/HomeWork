using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework1._1
{
    class RangeException : Exception
    {
        public int Mark { get; }
        public override string Message
        {
            get
            {
                return "Оценка введена неверно (оценка должна быть в диапазоне от 60 до 100)!!!";
            }
        }
        public RangeException(int mark)
        {
            Mark = mark;
        }
    }

    class ExistException : Exception
    {
        public string Name { get; }
        public string LastName { get; }
        public override string Message
        {
            get
            {
                return "Студенты с одинаковым именем и фамилией не могут дважды добавляться в список студентов!!!";
            }
        }

        public ExistException(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
    }

    class Student
    {
        public string Name { get; }
        public string LastName { get; }

        public Student(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
        public class CompareByName : IComparer<Student>
        {
            public int Compare(Student a, Student b)
            {
                if (a.LastName == b.LastName)
                    return String.Compare(a.Name, b.Name);
                else                        
                    return String.Compare(a.LastName, b.LastName);
            }
        }
    }
    
    class DictionaryStudents : IEnumerable<KeyValuePair<Student,int>>
    {
        SortedDictionary<Student, int> studentsDictionary = new SortedDictionary<Student, int>(new Student.CompareByName());

        public IEnumerator<KeyValuePair<Student, int>> GetEnumerator()
        {
            foreach (var st in studentsDictionary)
            {
                yield return st;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[Student index]
        {
            get
            {
                return studentsDictionary[index];
            }
            set
            {
                studentsDictionary[index] = value;
            }
        }

        public void AddStudent(Student student, int mark)
        {
            if (studentsDictionary.Count == 0)
            {
                studentsDictionary.Add(student, mark);
            }
            else
            { 
                try
                {
                    bool bol = true;
                    foreach (KeyValuePair<Student, int> en in studentsDictionary)
                    {
                        if ((student.Name.Equals(en.Key.Name)) && (student.LastName.Equals(en.Key.LastName)))
                        {
                            bol = false;
                            throw new ExistException(student.Name, student.LastName);
                        }
                    }
                    if (bol)
                    {
                        studentsDictionary.Add(student, mark);
                    }
                }
                catch (ExistException e)
                {
                    Console.WriteLine("Исключение: {0}", e.Message);
                    Console.WriteLine("Студент {0} {1} уже есть в списке студентов", e.Name, e.LastName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Исключение: {0}", e.Message);
                }
            }
        }

        public void AddStudents(int n)
        {
            for (int i = 0; i < n; i++)
            {
                try
                {
                    Console.WriteLine("Введите имя студента: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите фамилию студента: ");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("Введите оценку данного студента за экзамен: ");
                    int markTemp = Int32.Parse(Console.ReadLine());
                    if (markTemp < 60 | markTemp > 101)
                    {
                        throw new RangeException(markTemp);
                    }
                    else
                    {
                        Student student = new Student(name, lastName);
                        AddStudent(student, markTemp);
                    }
                }
                catch (RangeException e)
                {
                    Console.WriteLine("Исключение: ", e.Message);
                    Console.WriteLine("Оценка {0} не входит в диапазон от 60 до 100", e.Mark);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Исключение: ",e.Message);
                }
            }
        }
        public void ListStudents()
        {
            Console.WriteLine("Список студентов: ");
            foreach (KeyValuePair<Student, int> entry in studentsDictionary)
                Console.WriteLine("{0} {1} оценка: {2}", entry.Key.Name, entry.Key.LastName, entry.Value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DictionaryStudents dictionaryStudents = new DictionaryStudents();
            dictionaryStudents.AddStudents(3);
            dictionaryStudents.ListStudents();

           

        } 
    }
}
