using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;

// TODO: write unit tests for writing / reading
public class Expenses
{
	public List<Record> Records { get; private set; }
	public string FileName = "Expenses.json";

	public Expenses(string fileName)
	{
		Records = new List<Record>();
		if (fileName != "") FileName = fileName;
		ReadRecords();
	}

	public Expenses(List<Record> records)
	{
		Records = records;
	}

	// TODO: make Write/Read private and call them only inside of the command methods
	public void WriteRecords(string fileName)
	{
		//TODO: Before writing the record into .json, check that each guid is unique

		// if (!File.Exists(fileName)) File.Create(fileName).Dispose();

		string filePath = Path.GetFullPath(fileName);
		Console.WriteLine(filePath);

		var json = JsonSerializer.Serialize(Records, new JsonSerializerOptions { WriteIndented = true });
		Console.WriteLine(json);
		File.WriteAllText(fileName, json);
		Console.WriteLine("Records written to " + fileName);
	}

	public void ReadRecords()
	{

		if (!File.Exists(FileName))
		{
			using (var stream = File.Create(FileName))
			{
				Console.WriteLine($"File {FileName} created.");
			}
			return;
		}

		string jsonStr = File.ReadAllText(FileName);

		// The ! deletes the null warning, since checking if records is null in the next if statement
		Records = JsonSerializer.Deserialize<List<Record>>(jsonStr)!;

		// Check if the deserialization was succesful
		if (Records == null) throw new ArgumentException($"Error: deserialization of {FileName} unsuccseful");

		// Check if the .json isn't empty
		if (Records.Count == 0) throw new ArgumentException($"Error: {FileName} is empty, no records were loaded");

	}

	public void AddNewRecord(Dictionary<Enum, object> options)
	{
		// Get values -> declare here to be more readable
		double amount = (double)options[AddOpts.Amount];
		string name = (string)options[AddOpts.Name];
		Category category = (Category)options[AddOpts.Category];
		DateTime date = (DateTime)options[AddOpts.Date];
		int index = 0;

		if (Records.Count != 0) index = Records[Records.Count - 1].Index + 1;

		// Add the record
		Records.Add(new Record(index, name, amount, date, category));

		// Store into file
		WriteRecords(this.FileName);
	}

	public void ShowRecords(Dictionary<Enum, object> options)
	{

		// Chekc for Enum type validity
		if (options.Keys.First() is not ShowOpts)
			throw new ArgumentException("Internal Error: wrong dictionary format passed to method FilterRecords()");

		// Options can containt type NONE iff: the user didnt pass any options, meaning the parsed options must be only of length 1
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
		// Get values for date min,max
		DateTime dateFrom = (DateTime)(options.ContainsKey(ShowOpts.DateFrom) ?
			options[ShowOpts.DateFrom] :
			(DateTime)GetDefaultMinValue<DateTime>());


		DateTime dateTo = (DateTime)(options.ContainsKey(ShowOpts.DateTo) ?
			options[ShowOpts.DateTo] :
			(DateTime)GetDefaultMaxValue<DateTime>());

		List<Record> filtered = FilterRecords<DateTime>(Records, options, dateFrom, dateTo);

		// Get values for amount min,max
		double amountFrom = (double)(options.ContainsKey(ShowOpts.AmountFrom) ?
			options[ShowOpts.AmountFrom] :
			(double)GetDefaultMinValue<double>());

		double amountTo = (double)(options.ContainsKey(ShowOpts.AmountTo) ?
			options[ShowOpts.AmountTo] :
			(double)GetDefaultMaxValue<double>());


		filtered = FilterRecords<double>(filtered, options, amountFrom, amountTo);

		PrintRecords(filtered);
	}

	private void PrintRecords(List<Record> records)
	{
		foreach (var rec in records)
		{
			rec.PrintRecord();
		}
		Console.WriteLine("---------------------------------");
	}

	private List<Record> FilterRecords<T>(List<Record> records, Dictionary<Enum, object> options, T minVal, T maxVal)
	where T : IComparable<T>
	{
		return records.Where(record =>
		{
			IComparable recordValue = (minVal is double) ? (IComparable)record.Amount : (IComparable)record.Date;
			return recordValue.CompareTo(minVal) >= 0 && recordValue.CompareTo(maxVal) <= 0;
		}).ToList();

	}

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

	public void RemoveRecord(Dictionary<Enum, object> options)
	{
		Console.WriteLine($"Removing record of ID {(int)options[RemOpts.Id]}");

		Records?.RemoveAll(record => record.Index == (int)options[RemOpts.Id]);

		WriteRecords(this.FileName);
	}

}