namespace ConsoleApp.Tests;

using System.Data;
using ConsoleApp;

public class ExpensesTests
{
    [Fact]
    public void WriteAndLoadRecords_test1()
    {

        String fileName = "expenses.json";

        List<Record> records =
        [
            new Record("Pizza", 250, DateTime.Parse("1.1.2020"), Category.Food),
            new Record("McDonalds", 250, DateTime.Parse("15.1.2020"), Category.Food),
            new Record("Netflix", 400, DateTime.Parse("1.2.2022"), Category.Subscriptions),
            new Record("Amazon", 350, DateTime.Parse("1.2.2022"), Category.Subscriptions),
            new Record("Badminton", 250, DateTime.Parse("11.5.2022"), Category.Fun),
            new Record("Cat food", 600, DateTime.Parse("11.6.2023"), Category.Pet),
        ];

        string filePath = Path.GetFullPath("expenses.json");
        Console.WriteLine(filePath);

        Expenses expenses = new Expenses(records);
        expenses.WriteRecords(fileName);

        Expenses loaded = new Expenses("expenses.json");

        Assert.Equal(6, loaded.Records?.Count);
        Assert.Equal(250, loaded.Records?[0].Amount);
        Assert.Equal("Netflix", loaded.Records?[2].Name);
        Assert.Equal(Category.Fun, loaded.Records?[4].Category);
        Assert.Equal(DateTime.Parse("11.6.2023"), loaded.Records?[5].Date);
    }
}