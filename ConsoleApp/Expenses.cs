using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;

/// <summary>
/// Class representing all of the expenses for one .json file
/// </summary>
public class Expenses
{
	// List of all records of the expenses list
	public List<Record> Records { get; private set; }
	// Name of the file which contains expenses (default Expenses.json)
	public string FileName = "Expenses.json";

	/// <summary>
	/// Constructor, based on passed fileName reads all records from that file, if file is non existant, it creates
	/// new one with the name contained in fileName
	/// </summary>
	/// <param name="fileName">Name of file containing records</param>
	public Expenses(string fileName)
	{
		Records = new List<Record>();
		if (fileName != "") FileName = fileName;
		ReadRecords();
	}

	/// <summary>
	/// Constructor, creates records list from passed list of records -> DEBUG PURPOSES
	/// </summary>
	/// <param name="records">List of records</param>
	public Expenses(List<Record> records)
	{
		Records = records;
	}

	/// <summary>
	/// Method takes the List<Record> Records and deserializes it into a .json file and stores this data
	/// </summary>
	/// <param name="fileName">Name of the file to store the data in</param>
	public void WriteRecords(string fileName)
	{
		// Get path
		string filePath = Path.GetFullPath(fileName);

		// Serialize the list of records into a json format
		var json = JsonSerializer.Serialize(Records, new JsonSerializerOptions { WriteIndented = true });

		// Write into a file
		File.WriteAllText(fileName, json);
	}

	/// <summary>
	/// Method reads all records from a .json file which was setup via the constructor
	/// </summary>
	/// <exception cref="ArgumentException"></exception>
	public void ReadRecords()
	{
		// Check if the file exists, if not create it
		if (!File.Exists(FileName))
		{
			using (var stream = File.Create(FileName))
			{
				Console.WriteLine($"File {FileName} created.");
			}
			return;
		}

		// Read the .json file
		string jsonStr = File.ReadAllText(FileName);

		// Check if the file is empty (or only whitespace)
		if (string.IsNullOrWhiteSpace(jsonStr))
		{
			// If the file is empty, initialize Records to an empty list
			Records = new List<Record>();
		}
		else
		{
			// Otherwise, deserialize the JSON string into the Records list
			Records = JsonSerializer.Deserialize<List<Record>>(jsonStr) ?? new List<Record>();
		}

		// Check if the deserialization was succesful
		if (Records == null) throw new ArgumentException($"Error: deserialization of {FileName} unsuccseful");

	}

	/// <summary>
	/// Method adds a new record into the list of records
	/// </summary>
	/// <param name="options">Parsed options of command "add"</param>
	public void AddNewRecord(Dictionary<Enum, object> options)
	{
		// Get values -> declare here to be more readable
		double amount = (double)options[AddOpts.Amount];
		string name = (string)options[AddOpts.Name];
		Category category = (Category)options[AddOpts.Category];
		DateTime date = (DateTime)options[AddOpts.Date];
		int index = 0;

		// Check that it's not an empty list, if not give new index
		if (Records.Count != 0) index = Records[Records.Count - 1].Index + 1;

		// Add the record
		Records.Add(new Record(index, name, amount, date, category));

		// Store into file
		WriteRecords(this.FileName);
	}

	/// <summary>
	/// Method removes a record from list of records
	/// </summary>
	/// <param name="options">Dictionary of options passed via command line</param>
	public void RemoveRecord(Dictionary<Enum, object> options)
	{
		Console.WriteLine($"Removing record of ID {(int)options[RemOpts.Id]}");

		Records?.RemoveAll(record => record.Index == (int)options[RemOpts.Id]);

		WriteRecords(this.FileName);
	}

	/// <summary>
	/// Method shows all existing records
	/// </summary>
	/// <param name="options">Parsed options of command "show"</param>
	/// <exception cref="ArgumentException"></exception>
	public void ShowRecords(Dictionary<Enum, object> options)
	{

		// Chekc for Enum type validity
		if (options.Keys.First() is not ShowOpts)
			throw new ArgumentException("Internal Error: wrong dictionary format passed to method FilterRecords()");

		// Options can containt type NONE iff: the user didnt pass any options, 
		// meaning the parsed options must be only of length 1
		if (options.ContainsKey(ShowOpts.None))
		{
			if (options.Count() == 1)
			{
				PrintRecords(Records);
				return;
			}
			else
			{
				throw new ArgumentException("Internal Error: type none in options dictionary");
			}
		}

		// TODO: generalize -> have a method that does the min/max for both based on type passed.

		// Get value of dateFrom
		DateTime dateFrom = (DateTime)(options.ContainsKey(ShowOpts.DateFrom) ?
			options[ShowOpts.DateFrom] :
			(DateTime)GetDefaultMinValue<DateTime>());

		// Get value of dateTo
		DateTime dateTo = (DateTime)(options.ContainsKey(ShowOpts.DateTo) ?
			options[ShowOpts.DateTo] :
			(DateTime)GetDefaultMaxValue<DateTime>());

		// Filter records based on dateFrom and dateTo
		List<Record> filtered = FilterRecords<DateTime>(Records, dateFrom, dateTo);

		// Get value of amountFrom
		double amountFrom = (double)(options.ContainsKey(ShowOpts.AmountFrom) ?
			options[ShowOpts.AmountFrom] :
			(double)GetDefaultMinValue<double>());

		// Get value of amountTo
		double amountTo = (double)(options.ContainsKey(ShowOpts.AmountTo) ?
			options[ShowOpts.AmountTo] :
			(double)GetDefaultMaxValue<double>());

		// Filter records based on amountFrom and amountTo
		filtered = FilterRecords<double>(filtered, amountFrom, amountTo);

		// Print the filtered records
		PrintRecords(filtered);
	}

	/// <summary>
	/// Method prints passed list of records
	/// </summary>
	/// <param name="records">List of records to be printed</param>
	private void PrintRecords(List<Record> records)
	{
		foreach (var rec in records)
		{
			rec.PrintRecord();
		}
		Console.WriteLine("---------------------------------");
	}

	/// <summary>
	/// Generalized method to filter records based on passed parameters
	/// </summary>
	/// <typeparam name="T">IComparable, in this case either DateTime/Double</typeparam>
	/// <param name="records">List of records to be filtered</param>
	/// <param name="options">Dictionary of options</param>
	/// <param name="minVal">Lower bound for filter</param>
	/// <param name="maxVal">Upper bound for filter</param>
	/// <returns>Filtered records based on parameters passed</returns>
	private List<Record> FilterRecords<T>(List<Record> records, T minVal, T maxVal)
	where T : IComparable<T>
	{
		return records.Where(record =>
		{
			// get either the property amount or date based on T minVal passed
			IComparable recordValue = (minVal is double) ? (IComparable)record.Amount : (IComparable)record.Date;

			// Apply upper/lower bound and return
			return recordValue.CompareTo(minVal) >= 0 && recordValue.CompareTo(maxVal) <= 0;
		}).ToList();

	}

	/// <summary>
	/// Method takes in type (either DateTime or Double) and returns its maximum value
	/// </summary>
	/// <typeparam name="T">DateTime/Double</typeparam>
	/// <returns>Maximum value</returns>
	/// <exception cref="ArgumentException">If other types then DateTime or Double are passed</exception>
	public static IComparable GetDefaultMaxValue<T>()
	{
		if (typeof(T) == typeof(double))
		{
			return double.MaxValue;
		}
		else if (typeof(T) == typeof(DateTime))
		{
			return DateTime.MaxValue;
		}

		throw new ArgumentException("Internal Error: unknown type passed to GetDefaultMaxValue() method");
	}

	/// <summary>
	/// Method takes in type (either DateTime or Double) and returns its maximum value
	/// </summary>
	/// <typeparam name="T">DateTime/Double</typeparam>
	/// <returns>Minimum value</returns>
	/// <exception cref="ArgumentException">If other types then DateTime or Double are passed</exception>
	public static IComparable GetDefaultMinValue<T>()
	{
		if (typeof(T) == typeof(double))
		{
			return double.MinValue;
		}
		else if (typeof(T) == typeof(DateTime))
		{
			return DateTime.MinValue;
		}

		throw new ArgumentException("Internal Error: unknown type passed to GetDefaultMinValue() method");
	}
}