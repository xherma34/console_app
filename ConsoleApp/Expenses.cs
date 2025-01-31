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

		// Check if the deserialization was succesful
		if (Records == null) throw new ArgumentException($"Error: non-existing .json  file {fileName}");

		// Check if the .json isn't empty
		if (Records.Count == 0) throw new ArgumentException($"Error: {fileName} is empty, no records were loaded");

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