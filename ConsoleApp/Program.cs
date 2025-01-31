using System;
using System.IO;

public enum Category
{
	Fun,
	Rent,
	Food,
	Pet,
	Bills,
	Subscriptions,
	None
}


class Program
{
	static void Main(string[] args)
	{

		if (args.Length == 0)
		{
			Console.Error.WriteLine("Error: No arguments passed");
		}

	}

	public static void PrintDictionary<T>(Dictionary<T, object> dict)
	{
		foreach (var kvp in dict)
		{
			Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value} \n");
		}
	}

	static void PrintHelp()
	{
		Console.WriteLine("You can run the program with these commands:");
		Console.WriteLine("[ACTION] [OPTIONS]");
		Console.WriteLine("For each call only one action can be executed");
		Console.WriteLine("Actions:");
		Console.WriteLine("\n -> add -n \"name\" -c \"category\" -a \"amount\" -d \"date\" -o \"output file name\"");
		Console.WriteLine("\t ADDS a new expanse into the output file");
		Console.WriteLine("\t if no date is added it is added automatically to todays date");
		Console.WriteLine("\t if no output file is passed, it is automatically saved into the default expanses.json file");
		Console.WriteLine("\t name and category are required options");
		Console.WriteLine("\n\n-> remove \"ID\" \"file name\"");
		Console.WriteLine("\t REMOVES record with passed id from passed file");
		Console.WriteLine("\t Both options are required");
		Console.WriteLine("\n\n -> Show");

	}

}