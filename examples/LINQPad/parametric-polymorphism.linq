<Query Kind="Program" />

void Main()
{
	var value1 = "Hello World";
	var value1WithMo = Helper.AddMustache(value1);
	Console.WriteLine(value1WithMo);

	var value2 = 1234;
	var value2WithMo = Helper.AddMustache(value2);
	Console.WriteLine(value1WithMo);
	

	var value3 = new Person { Name = "John Doe" };
	var value3WithMo = Helper.AddMustache(value3);
	Console.WriteLine(value3WithMo);
	
}

// Define other methods and classes here

public static class Helper
{
	private const string Start = "{{";
	private const string End = "}}";
	
	public static string AddMustache<T>(T value)
	{
		return Start + value.ToString() + End;
	}
}

private class Person
{
	public string Name { get; set; }
	
	public override string ToString() {
		return Name;
	}
}