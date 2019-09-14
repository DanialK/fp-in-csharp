<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
  <Namespace>static LanguageExt.Prelude</Namespace>
  <Namespace>System</Namespace>
</Query>

void Main()
{
	var person = new Person("John", "Doe", new Role("Bartender", 50000));

	// Lenses
	var name = Person.firstName.Get(person);
	var personJnr1 = Person.firstName.Set(name + " Jnr", person);
	Console.WriteLine(personJnr1);

	var personJnr2 = Person.firstName.Update(n => n + " Jnr", person);
	Console.WriteLine(personJnr2);

	// Composing Lenses
	var cto = new Person("Joe", "Bloggs", new Role("CTO", 150000));
	var personSalary = lens(Person.role, Role.salary);
	var cto2 = personSalary.Set(170000, cto);
	Console.WriteLine(cto2);
}

// Define other methods and classes here


public class Person
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
	
	public Person With(
		string firstName = null,
		string lastName = null,
		Role role = null)
	{
		return new Person(
			firstName ?? FirstName,
			lastName ?? LastName,
			role ?? Role
		);
	}

	public static Lens<Person, string> firstName =>
		Lens<Person, string>.New(
			Get: p => p.FirstName,
			Set: x => p => p.With(firstName: x));

	public static Lens<Person, string> lastName =>
		Lens<Person, string>.New(
			Get: p => p.LastName,
			Set: x => p => p.With(lastName: x));

	public static Lens<Person, Role> role =>
		Lens<Person, Role>.New(
			Get: p => p.Role,
			Set: x => p => p.With(role: x));
}

public class Role
{
	public readonly string Title;
	public readonly int Salary;

	public Role(string title, int salary)
	{
		Title = title;
		Salary = salary;
	}

	public Role With(
		string title = null,
		int? salary = null)
	{
		return new Role(
			title ?? Title,
			salary ?? Salary
		);
	}

	public static Lens<Role, string> title =>
	Lens<Role, string>.New(
		Get: p => p.Title,
		Set: x => p => p.With(title: x));

	public static Lens<Role, int> salary =>
		Lens<Role, int>.New(
			Get: p => p.Salary,
			Set: x => p => p.With(salary: x));
}









