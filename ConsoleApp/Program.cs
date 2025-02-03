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

/// <summary>
/// Main program class
/// </summary>
class Program
{
	static void Main(string[] args)
	{

		// Check number of arguments passed
		if (args.Length == 0)
		{
			throw new ArgumentException("Error: no arguments passed");
		}

		// Dictionary to store command options
		Dictionary<Enum, object> options = new Dictionary<Enum, object>();

		// Parsing
		try
		{
			options = InputParser.ParseArguments(args);
		}
		catch (ArgumentException e)
		{
			Console.WriteLine(e);
			return;
		}


		// Get the enum type -> what command was passed
		Type enumType = options.Keys.First().GetType();


		// Get file name to load fileName .json file records
		string fileName = GetFileName(options);
		Expenses expenses = new Expenses(fileName);

		// Execute logic based on command passed from user
		if (options.Keys.First() is AddOpts)
		{
			expenses.AddNewRecord(options);
		}
		else if (options.Keys.First() is ShowOpts)
		{
			expenses.ShowRecords(options);
		}
		else if (options.Keys.First() is RemOpts)
		{
			expenses.RemoveRecord(options);
		}


	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="options">Dictionary of command options</param>
	/// <returns>File name extracted from options dictionary</returns>
	/// <exception cref="ArgumentException"></exception>
	private static string GetFileName(Dictionary<Enum, object> options)
	{
		// Error handle
		if (options == null || options.Count == 0)
			throw new ArgumentException("Internal error: Options dictionary is empty or null.");

		// A dictionary to map Enum types to their respective FileName keys
		var enumToFileNameMap = new Dictionary<Type, Enum>
		{
			{ typeof(AddOpts), AddOpts.FileName },
			{ typeof(RemOpts), RemOpts.FileName },
			{ typeof(ShowOpts), ShowOpts.FileName }
		};

		// Find the type of the first key (we assume only one type of enum will be present)
		var enumType = options.Keys.First().GetType();

		// If the type is not recognized, throw an error
		if (!enumToFileNameMap.ContainsKey(enumType))
			throw new ArgumentException("Internal error: Cannot parse Options dictionary");

		// Get the corresponding FileName enum value for this enum type
		Enum fileNameKey = enumToFileNameMap[enumType];

		// Try to get the file name from the dictionary
		if (options.TryGetValue(fileNameKey, out object? value))
		{
			return value as string ?? "";
		}

		return ""; // Return an empty string if no file name was found
	}

	/// <summary>
	/// -h help print message
	/// </summary>
	public static void PrintHelp()
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