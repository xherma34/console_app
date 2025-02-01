using System;
using System.Text;
using System.Text.Json;

// TODO: write unit tests for writing / reading
public class Expenses
{
	public List<Record>? Records { get; private set; }
	public string FileName = "Expenses.json";

	public Expenses(string fileName)
	{
		if (fileName != "") FileName = fileName;
		ReadRecords();
	}

	public Expenses(List<Record> records)
	{
		Records = records;
	}

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
			Records = new List<Record>();
			return;
		}

		string jsonStr = File.ReadAllText(FileName);

		Records = JsonSerializer.Deserialize<List<Record>>(jsonStr);

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

		if (Records?.Count != 0) index = Records[Records.Count - 1].Index + 1;

		// Add the record
		Records?.Add(new Record(index, name, amount, date, category));

		// Store into file
		WriteRecords(this.FileName);
	}

	private void FilterRecords(Dictionary<ShowOpts, object> options)
	{
		// TODO: Check logic here:
		// From < To for both, etc. 
		throw new NotImplementedException();
	}

	private void RemoveRecord(int id)
	{
		// TODO: 
		throw new NotImplementedException();
	}

}