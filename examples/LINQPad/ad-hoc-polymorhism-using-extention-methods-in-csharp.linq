<Query Kind="Program" />

void Main()
{
	/* We can almost get most of the benefits of Ad-Hoc Polymorphism using Extention methods in C# */
	var int1 = 6;
	var int2 = 9;
	Console.WriteLine(int1.Append(int2));


	var string1 = "6";
	var stirng2 = "9";
	Console.WriteLine(string1.Append(stirng2));


	Console.WriteLine(Helper.AppendTwice(int1, int2));
	Console.WriteLine(Helper.AppendTwice(string1, stirng2));
}

// Define other methods and classes here

public static class IntAppendableExtentions
{
	public static int Append(this int a, int b)
	{
		return a + b;
	}
}

public static class  AppendableString
{
	public static string Append(this string a, string b)
	{
		return $"{a}{b}";
	}
}


public static class Helper
{
	public static int AppendTwice(int a, int b)
	{
		var value = a.Append(b);
		return value.Append(value);
	}

	public static string AppendTwice(string a, string b)
	{
		var value = a.Append(b);
		return value.Append(value);
	}
}
