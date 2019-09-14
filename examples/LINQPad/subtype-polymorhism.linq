<Query Kind="Program" />

void Main()
{
	
	var int1 = new MyInt(6);
	var int2 = new MyInt(9);
	Console.WriteLine(int1.Append(int2).Value);


	var string1 = new MyString("6");
	var stirng2 = new MyString("9");
	Console.WriteLine(string1.Append(stirng2).Value);


	Console.WriteLine(Helper.AppendTwice(int1, int2).Value);
	Console.WriteLine(Helper.AppendTwice(string1, stirng2).Value);
}

// Define other methods and classes here

public abstract class Appendable<T>
{
	public abstract T Append(T b);
}

public class MyInt : Appendable<MyInt>
{
	public int Value { get; }
		
	public MyInt(int value)
	{
		Value = value;
	}
		
	public override MyInt Append(MyInt b)
	{
		return new MyInt(Value + b.Value);
	}
}


public class MyString : Appendable<MyString>
{
	public string Value { get; }

	public MyString(string value)
	{
		Value = value;
	}

	public override MyString Append(MyString b)
	{
		return new MyString($"{Value}{b.Value}");
	}
}


public static class Helper {
	public static T AppendTwice<T>(T a, T b) where T: Appendable<T>
	{
		var value = a.Append(b);
		return value.Append(value);
	}
}