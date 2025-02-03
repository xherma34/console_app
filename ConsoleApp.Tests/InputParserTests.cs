using System;

namespace ConsoleApp.Tests;

public class InputParserTests
{

	[Fact]
	public void AddCommand_OptValues_Test()
	{
		// Valid call
		string cmd0 = "add -c fun -a 200 -d 1.1.2024 -n name -o Test.json";
		// Invalid -c [CATEGORY]
		string cmd1 = "add -c xx -a 200 -d 1.1.2024 -n name -o Test.json";
		// Invalid -a [AMOUNT]
		string cmd2 = "add -c fun -a xx -d 1.1.2024 -n name -o Test.json";
		// Invalid -d [DATETIME]
		string cmd3 = "add -c fun -a 200 -d xx -n name -o Test.json";
		// Invalid -d [DATETIME]
		string cmd4 = "add -c fun -a 200 -d 20 -n name -o Test.json";
		// Invalid -a [AMOUNT] (negative number)
		string cmd5 = "add -c fun -a -200 -d xx -n name -o Test.json";

		InputParser.ParseArguments(cmd0.Split(' '));

		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd2.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd3.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd4.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd5.Split(' ')));

		// IDEA: maybe assert.equal that the messages are what is expected?

	}

	[Fact]
	public void AddCommand_OptionMissingValue_Test()
	{
		// Valid command
		// string cmd0 = "add -c fun -a 200 -d 1.1.2024 -n name";
		// Missing a value of -n
		string cmd1 = "add -c fun -a 200 -d 1.1.2024 -n";
		// Missing a value of -c
		string cmd2 = "add -c -a 200 -d 1.1.2024 -n name";
		// Missing a value of -d
		string cmd3 = "add -c fun -a 200 -d -n name";
		// Missing a value of -a
		string cmd4 = "add -c fun -a -d 1.1.2024 -n name";

		string cmd5 = "add";
		string cmd6 = "add -a 200";

		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd2.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd3.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd4.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd5.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd6.Split(' ')));
	}

	[Fact]
	public void AddCommand_MissingRequiredOpt_Test()
	{
		// Valid command
		// string cmd0 = "add -c fun -a 200 -d 1.1.2024 -n name";
		// Missing a value of -n
		string cmd1 = "add -c fun -a 200 -d 1.1.2024";
		// Missing a value of -c
		string cmd2 = "add -a 200 -d 1.1.2024 -n name";
		// Missing a value of -d
		string cmd3 = "add -c fun -a 200 -n name";
		// Missing a value of -a
		string cmd4 = "add -c fun -d 1.1.2024 -n name";

		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd2.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd3.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd4.Split(' ')));
	}

	[Fact]
	public void RemoveCommand_OptValue_Test()
	{
		// Valid command
		string cmd0 = "remove -id 1";
		// Invalid value (positive integer only)
		string cmd1 = "remove -id -1";
		// Invalid option
		string cmd2 = "remove -x xx";
		// Missing value
		string cmd3 = "remove -id";
		// Invalid value type (positive integer only)
		string cmd4 = "remove -id xx";

		InputParser.ParseArguments(cmd0.Split(' '));

		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd2.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd3.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd4.Split(' ')));
	}

	[Fact]
	public void RemoveCommand_MissingRequiredOpt_Test()
	{
		// Valid command
		// string cmd0 = "remove -id 1";
		// Missing -id
		string cmd1 = "remove";
		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
	}

	[Fact]
	public void ShowCommand_ValidOptionCombinations_Test()
	{
		// Valid command
		string cmd0 = "show -afrom 200 -ato 500 -dfrom 1.1.2000 -dto 1.1.2001";
		// Valid command
		string cmd1 = "show -afrom 200 -dfrom 1.1.2000";
		// Valid command
		string cmd2 = "show -afrom 200 -dto 1.1.2001";
		// Valid command
		string cmd3 = "show -afrom 300";
		// Valid command
		string cmd4 = "show -ato 500";
		// Valid command
		string cmd5 = "show -dfrom 1.1.2000";
		// Valid command
		string cmd6 = "show -dto 1.1.2001";
		string cmd7 = "show";

		InputParser.ParseArguments(cmd0.Split(' '));
		InputParser.ParseArguments(cmd1.Split(' '));
		InputParser.ParseArguments(cmd2.Split(' '));
		InputParser.ParseArguments(cmd3.Split(' '));
		InputParser.ParseArguments(cmd4.Split(' '));
		InputParser.ParseArguments(cmd5.Split(' '));
		InputParser.ParseArguments(cmd6.Split(' '));
		InputParser.ParseArguments(cmd7.Split(' '));
	}

	[Fact]
	public void ShowCommand_OptValue_Test()
	{
		// TODO: CHECK -ATO/-AFROM VALUES ARE >=0
		// Invalid amount type
		string cmd1 = "show -afrom xx";
		// Invalid amount type
		string cmd2 = "show -afto xx";
		// Invalid amount value
		string cmd3 = "show -ato -200";
		// Invalid amount value
		string cmd4 = "show -afrom -200";

		// Invalid date type
		string cmd5 = "show -dfrom xx";
		// Invalid date type
		string cmd6 = "show -dto xx";

		var exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd1.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd2.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd3.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd4.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd5.Split(' ')));
		exception = Assert.Throws<ArgumentException>(() => InputParser.ParseArguments(cmd6.Split(' ')));
	}
}
