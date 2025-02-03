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
			throw new ArgumentException("Error: No command passed to the program.");
		}

		string command = args[0].ToLower();

		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

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
				throw new ArgumentException("Error: Invalid command passed to the program");
		}

		if (!ContainsAllReqOpts(opts))
		{
			throw new ArgumentException("Error: Command doesn't contain all required options.");
		}

		return opts;
	}

	// TODO: write tests for this
	private static Dictionary<Enum, object> ParseAddOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		if (args.Length == 1) throw new ArgumentException("Error: Invalid command structure.");

		// Get the dictionary of options
		for (int i = 1; i < args.Length; i++)
		{
			switch (args[i].ToLower())
			{
				case "-n":
					if (opts.ContainsKey(AddOpts.Name) || i + 1 >= args.Length || IsOption(args[i + 1]))
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					opts.Add(AddOpts.Name, args[i + 1]);
					i++;
					break;

				case "-a":
					if (opts.ContainsKey(AddOpts.Amount) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;

					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double value))
					{
						throw new ArgumentException("Error: Invalid value type for option.");
					}

					if (value <= 0) throw new ArgumentException("Error: Invalid value for option.");

					opts.Add(AddOpts.Amount, value);
					break;

				case "-d":
					if (opts.ContainsKey(AddOpts.Date) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime date))
					{
						throw new ArgumentException("Error: Invalid value type for option.");
					}
					opts.Add(AddOpts.Date, date);
					break;

				case "-o":
					if (opts.ContainsKey(AddOpts.FileName) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					opts.Add(AddOpts.FileName, args[i]);
					break;

				case "-c":
					if (opts.ContainsKey(AddOpts.Category) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					Category category = GetCategory(args[i]);
					if (category == Category.None)
					{
						throw new ArgumentException("Error: Invalid value type for option.");
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
					if (opts.ContainsKey(ShowOpts.DateFrom) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime dateFrom))
					{
						throw new ArgumentException("Error: Invalid value type for option.");
					}
					opts.Add(ShowOpts.DateFrom, dateFrom);
					break;
				case "-dto":
					if (opts.ContainsKey(ShowOpts.DateTo) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					if (!DateTime.TryParse(args[i], out DateTime dateTo))
					{
						throw new ArgumentException("Error: Invalid value type for option.");
					}
					opts.Add(ShowOpts.DateTo, dateTo);
					break;
				case "-afrom":
					if (opts.ContainsKey(ShowOpts.AmountFrom) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double amountFrom))
					{
						throw new ArgumentException("Error: Invalid value type for option.");

					}
					if (amountFrom <= 0) throw new ArgumentException("Error: Invalid value for option.");
					opts.Add(ShowOpts.AmountFrom, amountFrom);
					break;
				case "-ato":
					if (opts.ContainsKey(ShowOpts.AmountTo) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					// Check if passed amount is a double value
					if (!double.TryParse(args[i], out double amountTo))
					{
						throw new ArgumentException("Error: Invalid value type for option.");
					}
					if (amountTo <= 0) throw new ArgumentException("Error: Invalid value for option.");
					opts.Add(ShowOpts.AmountTo, amountTo);
					break;
				case "-o":
					if (opts.ContainsKey(AddOpts.FileName) || i + 1 >= args.Length)
					{
						throw new ArgumentException("Error: Invalid command structure.");
					}
					i++;
					opts.Add(ShowOpts.FileName, args[i]);
					break;
				default:
					throw new ArgumentException($"Error: Invalid option {args[i]}.");
			}
		}

		return opts;
	}

	private static Dictionary<Enum, object> ParseRemoveOptions(string[] args)
	{
		Dictionary<Enum, object> opts = new Dictionary<Enum, object>();

		if (args.Length == 1) throw new ArgumentException("Error: Invalid command structure.");

		for (int i = 1; i < args.Length; i++)
		{
			if (args[i].ToLower() == "-id")
			{
				if (opts.ContainsKey(RemOpts.Id) || i + 1 >= args.Length)
					throw new ArgumentException("Error: Invalid command structure.");
				i++;
				if (!Int32.TryParse(args[i], out int value))
				{
					throw new ArgumentException("Error: Invalid value type for option.");
				}
				if (value < 0) throw new ArgumentException("Error: Invalid value for option.");
				opts.Add(RemOpts.Id, Int32.Parse(args[i]));
			}
			else if (args[i].ToLower() == "-o")
			{
				if (opts.ContainsKey(RemOpts.FileName) || i + 1 >= args.Length)
					throw new ArgumentException("Error: Invalid command structure.");
				i++;
				opts.Add(RemOpts.FileName, args[i]);
			}
			else
			{
				// Console.WriteLine($"Option being processed: {args[i].ToLower()}");
				throw new ArgumentException("Error: Invalid option.");
			}
		}

		return opts;
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