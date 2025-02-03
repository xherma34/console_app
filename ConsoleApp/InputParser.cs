using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Static class which parses the full command passed via command line
/// </summary>
public static class InputParser
{

	public static string ErrCommandFormat = "Error: Invalid command format, run with -h for help.";
	public static string ErrNoCommand = "Error: No command passed to the program.";
	public static string ErrReqOptions = "Error: Command doesn't contain all required options.";
	public static string ErrOptValType = "Error: Invalid value type for option.";
	public static string ErrInvalidOpt = "Error: Invalid option passed to the command.";


	/// <summary>
	/// Method parses input arguments
	/// </summary>
	/// <param name="args">Input arguments</param>
	/// <returns>Dictionary of parsed options</returns>
	public static Dictionary<Enum, object> ParseArguments(string[] args)
	{
		// Check if command was selected in input argument
		if (args.Length == 0)
		{
			throw new ArgumentException("Error: No command passed to the program.");
		}

		// Extract the command from input
		string command = args[0].ToLower();

		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		// Execute logic
		switch (command)
		{
			case "add":
				opts = ParseAddOptions(args);
				break;
			case "remove":
				opts = ParseRemoveOptions(args);
				break;
			case "show":
				opts = ParseShowOptions(args);
				break;

			default:
				throw new ArgumentException($"Error: {command} is not a valid command.");
		}

		// Check if opts contains all required options for the command
		if (!ContainsAllReqOpts(opts))
		{
			throw new ArgumentException(ErrReqOptions);
		}

		return opts;
	}

	/// <summary>
	/// Parses options from ADD command
	/// </summary>
	/// <param name="args">Input arguments</param>
	/// <returns>Parsed options</returns>
	private static Dictionary<Enum, object> ParseAddOptions(string[] args)
	{

		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		// Check for number of options
		if (args.Length == 1) throw new ArgumentException(ErrCommandFormat);

		// Get the dictionary of options
		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "-n":
					ParseOption(ref i, args, opts, AddOpts.Name, x => x);
					break;
				case "-a":
					ParseOption(ref i, args, opts, AddOpts.Amount, x => double.Parse(x));
					break;
				case "-d":
					ParseOption(ref i, args, opts, AddOpts.Date, x => DateTime.Parse(x));
					break;
				case "-o":
					ParseOption(ref i, args, opts, AddOpts.FileName, x => x);
					break;

				case "-c":
					ParseOption(ref i, args, opts, AddOpts.Category, x => GetCategory(x));
					break;

				default:
					break;
			}
		}

		return opts;
	}

	/// <summary>
	/// Parses options for SHOW command
	/// </summary>
	/// <param name="args">Input arguments</param>
	/// <returns>Parsed options</returns>
	private static Dictionary<Enum, object> ParseShowOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		// CASE: when the user only wants the .json to be printed and no filter is applied
		if (args.Length == 1)
		{
			// throw new ArgumentException($"Number of args XXX for {args.Length}");
			opts.Add(ShowOpts.None, "");
			return opts;
		}

		for (int i = 0; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "show":
					break;
				case "-dfrom":
					ParseOption(ref i, args, opts, ShowOpts.DateFrom, x => DateTime.Parse(x));
					break;
				case "-dto":
					ParseOption(ref i, args, opts, ShowOpts.DateTo, x => DateTime.Parse(x));
					break;
				case "-afrom":
					ParseOption(ref i, args, opts, ShowOpts.AmountFrom, x => double.Parse(x));
					break;
				case "-ato":
					ParseOption(ref i, args, opts, ShowOpts.AmountTo, x => double.Parse(x));
					break;
				case "-o":
					ParseOption(ref i, args, opts, ShowOpts.FileName, x => x);
					break;
				default:
					throw new ArgumentException($"Error: Invalid option {args[i]}.");
			}
		}

		return opts;
	}

	/// <summary>
	/// Parses options for REMOVE command
	/// </summary>
	/// <param name="args">Input arguments</param>
	/// <returns>Parsed options</returns>
	private static Dictionary<Enum, object> ParseRemoveOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		if (args.Length == 1) throw new ArgumentException(ErrCommandFormat);

		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "-id":
					ParseOption(ref i, args, opts, RemOpts.Id, x => Int32.Parse(x));
					break;
				case "-o":
					ParseOption(ref i, args, opts, RemOpts.FileName, x => x);
					break;
				default:
					throw new ArgumentException($"Error: Invalid option {args[i]}.");
			}
		}

		return opts;
	}

	/// <summary>
	/// Generalized method which gets option and its value from input arguments, checks for any error in the format
	/// and adds the option:value duo into the opts dictionary
	/// </summary>
	/// <typeparam name="T">Function type</typeparam>
	/// <param name="index">Index of option in array of input arguments</param>
	/// <param name="args">Array of input arguments</param>
	/// <param name="opts">Dictionary of option:value</param>
	/// <param name="optEnum">Enum type representing the type of option to be added into the dictionary</param>
	/// <param name="parseFunc">Lambda function for any option which needs type casting/parsing (DateTime, double etc.)</param>
	private static void ParseOption<T>(ref int index, string[] args, Dictionary<Enum, object> opts, Enum optEnum, Func<string, T> parseFunc)
	{
		// Check basic conditions: 
		// Not a redundant option value || not out of bounds || the value for parsed option is not another option
		if (opts.ContainsKey(optEnum) || index + 1 >= args.Length || IsOption(args[index + 1]))
		{
			throw new ArgumentException(ErrCommandFormat);
		}

		index++;

		T value;
		// Try to parse using the provided function
		if (TryParseArgument(args[index], parseFunc, out value))
		{
			if ((value is int intVal && intVal < 0) ||
				(value is double doubleVal && doubleVal < 0) ||
				(value is Category categoryVal && categoryVal == Category.None))
			{
				throw new ArgumentException(ErrOptValType);
			}

			if (value == null)
			{
				throw new ArgumentException($"Error: ParseOption() function parameter returned null");
			}

			// Add the value and option into the dictionary
			opts.Add(optEnum, value);
		}
		else
		{
			throw new ArgumentException(ErrCommandFormat);
		}

	}

	/// <summary>
	/// Method tries to perform parsing function passed
	/// </summary>
	/// <typeparam name="T">Type of the result</typeparam>
	/// <param name="input">Input string</param>
	/// <param name="parseFunc">Parse function which should parse the input string</param>
	/// <param name="result">Result of the parse function</param>
	/// <returns>True if input can be parsed via passed parse function</returns>
	private static bool TryParseArgument<T>(string input, Func<string, T> parseFunc, out T result)
	{
		try
		{
			result = parseFunc(input);
			return true;
		}
		catch
		{
#pragma warning disable CS8601 // Possible null reference assignment.
			result = default;
#pragma warning restore CS8601 // Possible null reference assignment.
			return false;
		}
	}

	/// <param name="opt"></param>
	/// <returns>True if string opt is one of the options</returns>
	private static bool IsOption(string opt)
	{
		switch (opt.ToLower())
		{
			case "-o":
			case "-d":
			case "-a":
			case "-n":
			case "-c":
				return true;
			default:
				return false;
		}
	}

	/// <param name="opts">Options dictionary</param>
	/// <returns>True if for passed enum the dictionary contains all required options</returns>
	private static bool ContainsAllReqOpts(Dictionary<Enum, object> opts)
	{
		if (opts.Keys.First() is AddOpts)
		{
			return opts.ContainsKey(AddOpts.Amount) &&
				opts.ContainsKey(AddOpts.Category) &&
				opts.ContainsKey(AddOpts.Date) &&
				opts.ContainsKey(AddOpts.Name);
		}
		else if (opts.Keys.First() is RemOpts)
		{
			return opts.ContainsKey(RemOpts.Id);
		}

		return true;
	}

	/// <summary>
	/// Parses string category into the enum Category
	/// </summary>
	/// <param name="str">String category</param>
	/// <returns>Category enum value</returns>
	private static Category GetCategory(string str)
	{
		switch (str.ToLower())
		{
			case "fun": return Category.Fun;
			case "rent": return Category.Rent;
			case "pet": return Category.Pet;
			case "bills": return Category.Bills;
			case "subs": return Category.Subscriptions;
			case "food":
				Console.WriteLine("Returning category food");
				return Category.Food;
			default:
				Console.Error.WriteLine("Error: invalid category passed");
				return Category.None;
		}

	}



}


public enum CmdType
{
	Add,
	Remove,
	Show,
	None
}

public enum AddOpts
{
	Date,
	Name,
	Amount,
	FileName,
	Category
}

public enum RemOpts
{
	Id,
	FileName,
	None
}

public enum ShowOpts
{
	DateFrom,
	DateTo,
	AmountFrom,
	AmountTo,
	FileName,
	None
}