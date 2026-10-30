using System;

namespace StudentRegister
{
    /// <summary>
    /// Notification delegate used when a mark is added to a student.
    /// </summary>
    /// <param name="msg">The message describing the added mark.</param>
    public delegate void MarkHandler(string msg);

    /// <summary>
    /// Stretch: a course code with its credits (value type).
    /// </summary>
    public struct CourseCode
    {
        /// <summary>The course code, e.g. "CPE317".</summary>
        public string Code;

        /// <summary>The credit value of the course.</summary>
        public int Credits;

        /// <summary>
        /// Creates a new <see cref="CourseCode"/>.
        /// </summary>
        /// <param name="code">The course code.</param>
        /// <param name="credits">The number of credits.</param>
        public CourseCode(string code, int credits)
        {
            Code = code;
            Credits = credits;
        }
    }

    /// <summary>
    /// Abstract base class. It cannot be instantiated directly.
    /// </summary>
    public abstract class Person
    {
        /// <summary>The person's name.</summary>
        public string Name;

        /// <summary>Protected field: visible only to Person and derived classes.</summary>
        protected int age;

        /// <summary>
        /// Creates a new <see cref="Person"/>.
        /// </summary>
        /// <param name="name">The person's name.</param>
        /// <param name="age">The person's age.</param>
        protected Person(string name, int age)
        {
            Name = name;
            this.age = age;
        }

        /// <summary>
        /// Prints a description of the person. Derived classes must override it.
        /// </summary>
        public abstract void Describe();
    }

    /// <summary>
    /// A student with a validated mark and a mark-added event.
    /// </summary>
    public class Student : Person
    {
        private int mark;

        /// <summary>The course this student is taking (stretch goal).</summary>
        public CourseCode Course;

        /// <summary>
        /// Raised exactly once every time a valid mark is set.
        /// </summary>
        public event MarkHandler? OnMarkAdded;

        /// <summary>
        /// The student's mark. The setter validates the range 0-100.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside 0-100.</exception>
        public int Mark
        {
            get { return mark; }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Mark must be between 0 and 100.");
                }

                mark = value;
                // Valid value -> fire the event exactly once.
                OnMarkAdded?.Invoke($"{Name} got a new mark: {mark}");
            }
        }

        /// <summary>
        /// Creates a new <see cref="Student"/> and calls the base constructor.
        /// </summary>
        /// <param name="name">The student's name.</param>
        /// <param name="age">The student's age.</param>
        /// <param name="course">The student's course.</param>
        public Student(string name, int age, CourseCode course) : base(name, age)
        {
            Course = course;
        }

        /// <summary>
        /// Prints the student's name and mark (overrides the abstract method).
        /// </summary>
        public override void Describe()
        {
            Console.WriteLine($"Student: {Name}, Mark: {Mark}, Course: {Course.Code} ({Course.Credits} credits)");
        }
    }

    /// <summary>
    /// Program entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates 2 students, subscribes to the event, adds marks and prints.
        /// </summary>
        public static void Main()
        {
            Student s1 = new Student("Ali", 20, new CourseCode("CPE317", 4));
            Student s2 = new Student("Ayse", 21, new CourseCode("CPE317", 4));

            // Subscribe to the event
            s1.OnMarkAdded += msg => Console.WriteLine("[EVENT] " + msg);
            s2.OnMarkAdded += msg => Console.WriteLine("[EVENT] " + msg);

            // Add valid marks (the event fires once for each)
            s1.Mark = 85;
            s2.Mark = 92;

            // Invalid mark -> rejected, event does NOT fire
            try
            {
                s1.Mark = 150;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Rejected: " + ex.Message);
            }

            // Print
            s1.Describe();
            s2.Describe();
        }
    }
}