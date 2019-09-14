using System;
using LanguageExt;

namespace FPinCSharp.ImmutableTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            var personV1 = new Person("John", "Doe", "Bartender");
            var personV2 = personV1.With(Occupation: "Engineer");

            Console.WriteLine(personV1.Occupation);
            Console.WriteLine(personV2.Occupation);
        }
    }


    [With]
    public partial class Person
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Occupation;

        public Person(string firstName, string lastName, string occupation)
        {
            FirstName = firstName;
            LastName = lastName;
            Occupation = occupation;
        }

    }
}
