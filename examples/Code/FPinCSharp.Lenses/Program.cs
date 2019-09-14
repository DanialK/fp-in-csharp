using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPinCSharp.Lenses
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person("John", "Doe", new Role("Bartender", 50000));


            // Lenses
            var name = Person.firstName.Get(person);
            var personJnr1 = Person.firstName.Set(name + " Jnr", person);

            var personJnr2 = Person.firstName.Update(n => n + " Jnr", person);


            Console.WriteLine(person.FirstName);
            Console.WriteLine(personJnr1.FirstName);
            Console.WriteLine(personJnr2.FirstName);

            // Composing Lenses
            var cto1 = new Person("Joe", "Bloggs", new Role("CTO", 150000));
            var personSalary = lens(Person.role, Role.salary);
            var cto2 = personSalary.Set(170000, cto1);

            Console.WriteLine(cto1.Role.Salary);
            Console.WriteLine(cto2.Role.Salary);
        }
    }

    [WithLens]
    public partial class Person
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly Role Role;

        public Person(string firstName, string lastName, Role role)
        {
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }
    }

    [WithLens]
    public partial class Role
    {
        public readonly string Title;
        public readonly int Salary;

        public Role(string title, int salary)
        {
            Title = title;
            Salary = salary;
        }
    }
}
