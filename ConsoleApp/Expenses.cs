using System;
using System.Text.Json;

// TODO: write unit tests for writing / reading
public class Expenses
{
	public List<Record>? Records { get; private set; }
	public string FileName = "Expenses.json";

	public Expenses(string fileName)
	{
		if (fileName != "") FileName = fileName;
		ReadRecords(fileName);
	}

	public Expenses(List<Record> records)
	{
		Records = records;
	}

	public void WriteRecords(string fileName)
	{
		//TODO: Before writing the record into .json, check that each guid is unique

		if (!File.Exists(fileName)) File.Create(fileName).Dispose();

		string filePath = Path.GetFullPath(fileName);
		Console.WriteLine(filePath);

		var json = JsonSerializer.Serialize(Records, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(fileName, json);
		Console.WriteLine("Records written to " + fileName);
	}

	public void ReadRecords(string fileName)
	{
		string jsonStr = File.ReadAllText(fileName);

		Records = JsonSerializer.Deserialize<List<Record>>(jsonStr);
		if (Records == null)
		{
			Console.Error.WriteLine($"Error: .json format passed in wrong format{fileName}");
		}

	}

}