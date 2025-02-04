# Console app - Expense tracker
#### Description
This app is built to keep track of expanses, where expenses differ in categories like: fun, food, rent, bills, subscriptions etc. (full list below). The app lets you add new expenses, print them into terminal with some filters applied or remove records from expenses. The core of the data managing are .json files, which are being read from/written into.

## Operating manual
### Get started
The application operates on .NET 8. 

To download this app clone this repository to your machine. After the cloning the app needs to be compiled, go ahead and execute `$dotnet build`. This compiles the whole program. 

If you want to try out and run the unit tests you need to move into the ConsoleApp.Tests/ project directory run `$dotnet build` and ``$dotnet test`` which executes the unit tests.

To run the application, move to the root directory and run 
``$dotnet run -- [COMMAND] [COMMAND OPTIONS]``.

### Commands and their options
#### Values
Each of the options has defined data types and formats:
- [CATEGORY] - subs || fun || pet || food || rent || bills
  - Category defines what type of expense it is, either food, pet accessories etc.
- [NAME] - string (without spaces)
  - Used for naming one expense.
- [DATE], [DFROM], [DTO] - Date time
  - Date of the expense in either format: dd.mm.yyyy || dd/mm/yyyy etc.
- [AMOUNT], [AFROM], [ATO] - double (noninegative)
  - Amount spent on the expense.
- [FILENAME] - string
  - Name of the file which either holds records of the expenses or a new file to be used for this purpose.
- [ID] - integer (non-negative)


#### 1. Add command
**ADD** is used to create new records, it has these options:
- Required options:
  - "-c" [CATEGORY]
  - "-n" [NAME]
  - "-a" [AMOUNT]
  - "-d" [DATE]
- Voluntary options:
  - "-o" [FILENAME]
    - File to be read from, if file is not passed the default is set to be *Expenses.json*

Example of how to use the command:
``$dotnet run -- add -c subs -n Netflix -a 500 -d 31/12/2024 -o expenses_2024.json``


**SHOW**: used to print out records into the terminal, options can filter out this output.
- **Required options:** NONE
- **Voluntary options:** used to filter the records you want to see
  - "-afrom" [AMOUNT] - amount from (must be lesser or equal to ato)
  - "-ato" [AMOUNT] - amount to (must be larger or equal to afrom)
  - "-dfrom" [DATE] - date from (must be lesser or equal to dto)
    - date from
  - "-dto" [DATE] - date to (must be larger or equal to dfrom)
  - "-o" [FILENAME] - file to be read

- **Option rules:**
Example of how to use the command:
``$dotnet run -- show -afrom 250 -ato 400 -dfrom 1.1.2024 -dto 1.2.2024 -o myfile.json``

**REMOVE**:
- required options: 
  - "-id" [ID]
- Voluntary options:
  - "-o" [FILENAME]

Example of how to use the command:
``$dotnet run -- remove -id 2 -o myfile.json``