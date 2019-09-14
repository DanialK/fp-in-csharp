<Query Kind="Program" />

void Main()
{
	var person = new Person("John", "Doe", "Bartender");
	
	Console.WriteLine(
		person.With(occupation: "Engineer")
	);
}

// Define other methods and classes here


public class Person
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
	
	public Person With(
		string firstName = null,
		string lastName = null,
		string occupation = null)
	{
		return new Person(
			firstName ?? FirstName,
			lastName ?? LastName,
			occupation ?? Occupation
		);
	}
}
