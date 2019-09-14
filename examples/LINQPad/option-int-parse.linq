<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
  <Namespace>static LanguageExt.Prelude</Namespace>
</Query>


void Main()
{
	Console.WriteLine(Int.Parse("100"));
	Console.WriteLine(Int.Parse("hello"));
	
	
	var inputs = new []
	{
		"1",
		"2",
		"hello",
		"world",
		"3"
	};

		
	inputs
		.Map(Int.Parse)
		.IterT(Console.WriteLine);
	
	var valid = inputs
		.Map(Int.Parse)
		.Somes();
	Console.WriteLine(valid);


	var cleaned = inputs
		.Map(Int.Parse)
		.Map(x => x.IfNone(0));
	Console.WriteLine(cleaned);
}

// Define other methods and classes here


public static class Int
{
	public static Option<int> Parse(string s)
	{
		return int.TryParse(s, out var result)
			? Some(result)
			: None;
	}
}
