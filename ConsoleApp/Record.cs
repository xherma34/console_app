using System;

/// <summary>
/// Class representing one record of the expenses
/// </summary>
public class Record
{
	public Guid ID { get; private set; }
	public string Name { get; private set; }
	public double Amount { get; private set; }
	public DateTime Date { get; private set; }
	public Category Category { get; private set; }
	public int Index { get; private set; }

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="index">id</param>
	/// <param name="name">Name of the payment</param>
	/// <param name="amount">Amount paid for record</param>
	/// <param name="date">Day of occurence of the payment</param>
	/// <param name="category">Category of payment</param>
	public Record(int index, string name, double amount, DateTime date, Category category)
	{
		ID = Guid.NewGuid();
		Name = name;
		Amount = amount;
		Date = date;
		Category = category;
		Index = index;
	}

	public void ChangeName(string name)
	{
		Name = name;
	}
	public void ChangeAmount(double amount)
	{
		Amount = amount;
	}
	public void ChangeDate(DateTime date)
	{
		Date = date;
	}
	public void ChangeCategory(Category category)
	{
		Category = category;
	}

	/// <summary>
	/// Prints out record information in a formatted style
	/// </summary>
	public void PrintRecord()
	{
		string dateStr = Date.ToString("dd/MM/yyyy");
		string catStr = Category.ToString();

		Console.WriteLine("---------------------------------");
		Console.WriteLine($"{"Index:",-10} {Index,20} |");
		Console.WriteLine($"{"Name:",-10} {Name,20} |");
		Console.WriteLine($"{"Date:",-10} {dateStr,20} |");
		Console.WriteLine($"{"Amount:",-10} {Amount,20:C} |");
		Console.WriteLine($"{"Category:",-10} {catStr,20} |");

	}

}