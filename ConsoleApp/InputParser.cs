using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Static class which parses the full command passed via command line
/// </summary>
public static class InputParser
{
	// TODO: Create unit tests
	// TODO: Refactor the switches in the parse methods to be more clean
	public static Dictionary<Enum, object> ParseArguments(string[] args)
	{

		if (args.Length == 0)
		{
			// TODO: implement try/catch blocks for this to work
			throw new ArgumentException("Error: no arguments were passed to the program");
		}

		string command = args[0].ToLower();

		switch (command)
		{
			case "add":
				return ParseAddOptions(args);
			case "remove":
			// return ParseRemoveOptions(args);
			case "show":
				return ParseShowOptions(args);

			default:
				throw new ArgumentException("Error: invalid command passed to the program");
		}

	}

	// TODO: write tests for this
	private static Dictionary<Enum, object> ParseAddOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		// Get the dictionary of options
		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "-n":
					if (opts.ContainsKey(AddOpts.Name) || i + 1 >= args.Length || IsOption(args[i + 1]))
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					opts.Add(AddOpts.Name, args[i + 1]);
					i++;
					break;

				case "-a":
					if (opts.ContainsKey(AddOpts.Amount) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double value))
					{
						throw new ArgumentException("Invalid -a [AMOUNT] passed, Amount must be a nubmer");

					}
					opts.Add(AddOpts.Amount, value);
					break;

				case "-d":
					if (opts.ContainsKey(AddOpts.Date) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime date))
					{
						throw new ArgumentException("Error: Invalid -d [DATE] date format passed");
					}
					opts.Add(AddOpts.Date, date);
					break;

				case "-o":
					if (opts.ContainsKey(AddOpts.FileName) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					opts.Add(AddOpts.FileName, args[i]);
					break;

				case "-c":
					if (opts.ContainsKey(AddOpts.Category) || i + 1 >= args.Length) return null;
					i++;
					Category category = GetCategory(args[i]);
					if (category == Category.None)
					{
						throw new ArgumentException("Error: Invalid -c [CATEGORY] passed");
					}
					opts.Add(AddOpts.Category, category);
					break;

				default:
					break;
			}


		}

		return opts;
	}

	// DateFrom,
	// DateTo,
	// AmountFrom,
	// AmountTo,
	// TODO: write tests for this
	private static Dictionary<Enum, object> ParseShowOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		for (int i = 0; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "-Dfrom":
					if (opts.ContainsKey(ShowOpts.DateFrom) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime dateFrom))
					{
						throw new ArgumentException("Error: Invalid -d [DATE] date format passed");
					}
					opts.Add(ShowOpts.DateFrom, dateFrom);
					break;
				case "-Dto":
					if (opts.ContainsKey(ShowOpts.DateTo) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime dateTo))
					{
						throw new ArgumentException("Error: Invalid -d [DATE] date format passed");
					}
					opts.Add(ShowOpts.DateTo, dateTo);
					break;
				case "-Afrom":
					if (opts.ContainsKey(ShowOpts.AmountFrom) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double amountFrom))
					{
						throw new ArgumentException("Invalid -a [AMOUNT] passed, Amount must be a nubmer");

					}
					opts.Add(ShowOpts.AmountFrom, amountFrom);
					break;
				case "-Ato":
					if (opts.ContainsKey(ShowOpts.AmountTo) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double amountTo))
					{
						throw new ArgumentException("Invalid -a [AMOUNT] passed, Amount must be a nubmer");

					}
					opts.Add(ShowOpts.AmountTo, amountTo);
					break;
				case "-o":
					if (opts.ContainsKey(AddOpts.FileName) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command syntax");
					}
					i++;
					opts.Add(ShowOpts.FileName, args[i]);
					break;
				default:
					break;
			}
		}

		return opts;
	}

	private static Dictionary<Category, object> ParseRemoveOptions(string[] args)
	{
		throw new NotImplementedException();
	}

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
	none
}

public enum ShowOpts
{
	DateFrom,
	DateTo,
	AmountFrom,
	AmountTo,
	FileName
}