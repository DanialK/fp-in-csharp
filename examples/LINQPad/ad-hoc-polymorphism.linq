<Query Kind="Program" />

void Main()
{
	/*
		From https://github.com/louthy/language-ext#ad-hoc-polymorphism:
		
		This is where things get a little crazy!
		This section is taking what's possible with C# to its limits.
		None of what follows is essential for 99% of the use cases for this library.
		However, the seasoned C# programmer will recognise some of the issues raised (like no common numeric base-type);
		and experienced functional programmers will recognise the category theory creeping in... Just remember, this is all optional
		but also pretty powerful if you want to go for it.
	*/
	var int1 = 6;
	var int2 = 9;
	Console.WriteLine(default(AppendableInt).Append(int1, int2));


	var string1 = "6";
	var stirng2 = "9";
	Console.WriteLine(default(AppendableString).Append(string1, stirng2));



	Console.WriteLine(Helper.AppendTwice<AppendableInt, int>(int1, int2));
	Console.WriteLine(Helper.AppendTwice<AppendableString, string>(string1, stirng2));


	/*
		NOTE: equivalent Scala code which is syntactically nicer due to
		the magic of scala implicits:
		
		trait Appendable[A] {
		  def append(a: A, b: A): A
		}
		
		object Appendable {
		  implicit val appendableInt = new Appendable[Int] {
		    override def append(a: Int, b: Int) = a + b
		  }
		  implicit val appendableString = new Appendable[String] {
		    override def append(a: String, b: String) = a.concat(b)
		  }
		}
		
		def appendItems[A](a: A, b: A)(implicit ev: Appendable[A]) =
		  ev.append(a, b)
		  
		val res1 = appendItems(2, 3) // returns 5
		val res2 = appendItems("2", "3") // returns "23"
	
	*/
}

// Define other methods and classes here
public interface Appendable<T>
{
    T Append(T a, T b);
}

public struct AppendableInt : Appendable<int>
{
	public int Append(int a, int b)
	{
		return a + b;
	}
}

public struct AppendableString : Appendable<string>
{
	public string Append(string a, string b)
	{
		return $"{a}{b}";
	}
}


public static class Helper
{
	public static T AppendTwice<AppendableT, T>(T a, T b) where AppendableT : struct, Appendable<T>
	{
		var value = default(AppendableT).Append(a, b);
		return default(AppendableT).Append(value, value);
	}
}


