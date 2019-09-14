<Query Kind="Program">
  <NuGetReference>LanguageExt.CodeGen</NuGetReference>
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
</Query>

void Main()
{
	var node1a = new Node(1, "PD");
	var node1b = new Node(1, "PD");
	var node2 = new Node(2, "PD");

	// Equality Checks (Eq)
	Console.WriteLine("node1a == node1a => " + (node1a == node1a));
	Console.WriteLine("node1a == node1b => " + (node1a == node1b));
	Console.WriteLine("node1a == node2 => " + (node1a == node2));
	Console.WriteLine("node1b == node2 => " + (node1b == node2));

	// Comparision (Ord)
	Console.WriteLine("node1a <= node1b => " + (node1a <= node1b));
	Console.WriteLine("node1a < node2 => " + (node1a < node2));
	Console.WriteLine("node1a > node2 => " + (node1a > node2));

	// Stringify (Show)
	Console.WriteLine(node1a.ToString());
	Console.WriteLine(node2.ToString());
}

// Define other methods and classes here


public class Node : Record<Node>
{
	public int Code { get; }
	public string ShortName { get; }

	public Node(int code, string shortName)
	{
		Code = code;
		ShortName = shortName;
	}
}

