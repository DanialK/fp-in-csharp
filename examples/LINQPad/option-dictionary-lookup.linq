<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
  <Namespace>static LanguageExt.Prelude</Namespace>
</Query>

void Main()
{
	var dict = Enumerable
		.Range(1, 10)
		.ToDictionary(x => x.ToString(), x => x);


	Helpers.PrintOption(dict.Lookup("1"));
	Helpers.PrintOption(dict.Lookup("10"));
	Helpers.PrintOption(dict.Lookup("20"));
}

// Define other methods and classes here
public static class DictionaryExt
{
	public static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
	{
		return dict.TryGetValue(key, out T value)
			? Some(value)
			: None;
	}
}

public static class Helpers
{
	public static void PrintOption<T>(Option<T> value)
	{
		var message = value.Match(
			None: () => "Oppps",
			Some: (x) => "This is " + x
		);
		
		Console.WriteLine(message);
	}
}