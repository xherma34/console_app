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

		Dictionary<Enum, object> options = InputParser.ParseArguments(args);

		// Get the enum type -> what command was passed
		Type enumType = options.Keys.First().GetType();

		Expenses expenses;

		string fileName = "";

		// TODO: refactor and use delegate here
		// TODO: write unit tests for ADD command parsing
		if (options.Keys.First() is AddOpts)
		{
			if (options.TryGetValue(AddOpts.FileName, out object value))
			{
				fileName = value as string ?? "";
			}

			expenses = new Expenses(fileName);
			expenses.AddNewRecord(options);

		}
		else if (options.Keys.First() is ShowOpts)
		{

		}
	}

	public static void AddNewRecord(Dictionary<Enum, object> options, Expenses expenses)
	{

	}

	public static void ShowRecords(Dictionary<Enum, object> options)
	{

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