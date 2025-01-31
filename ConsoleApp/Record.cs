using System;

public class Record
{
	public Guid ID { get; private set; }
	public string Name { get; private set; }
	public double Amount { get; private set; }
	public DateTime Date { get; private set; }
	public Category Category { get; private set; }
	public int Index { get; private set; }

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

}