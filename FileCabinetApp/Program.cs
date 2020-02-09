using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Kiryl Zhurauleu";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string, string>>[] commands = new Tuple<string, Action<string, string>>[]
        {
            new Tuple<string, Action<string, string>>("help", PrintHelp),
            new Tuple<string, Action<string, string>>("stat", Stat),
            new Tuple<string, Action<string, string>>("create", Create),
            new Tuple<string, Action<string, string>>("edit", Edit),
            new Tuple<string, Action<string, string>>("find", Find),
            new Tuple<string, Action<string, string>>("list", List),
            new Tuple<string, Action<string, string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "edit", "modifies existing records", "The 'exit' command modifies existing records." },
            new string[] { "find", "finds records by criterion ", "The 'exit' command finds records by criterion." },
            new string[] { "list", "return a list of records added to the service.", "The 'exit' command return a list of records added to the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters, " ");
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters, string filler = "")
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters, string filler = "")
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parametrs, string filler = "")
        {
            try
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();
                Console.Write("Last name: ");
                var lastName = Console.ReadLine();
                Console.Write("Date of birth: ");
                var dataOfBirth = Console.ReadLine();

                Console.Write("Gender: ");
                var gender = Convert.ToChar(Console.ReadLine());
                Console.Write("Expirience: ");
                var expirience = Convert.ToInt16(Console.ReadLine());
                Console.Write("Account: ");
                var account = Convert.ToDecimal(Console.ReadLine());

                DateTime date;
                CultureInfo iOCultureFormat = new CultureInfo("en-US");
                DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);

                var index = fileCabinetService.CreateRecord(firstName, lastName, date, gender, expirience, account);
                Console.WriteLine($"Record #{index} is created.");
            }
            catch
            {
                Console.WriteLine("Incorrectly entered data. Repeat entry again");
                Create(parametrs);
            }
        }

        private static void Edit(string parametrs, string filler = "")
        {
            var id = Convert.ToInt32(Console.ReadLine());
            if (Program.fileCabinetService.GetStat() < id)
            {
                Console.WriteLine("#id record is not found.");
                Edit(parametrs);
            }

            try
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();
                Console.Write("Last name: ");
                var lastName = Console.ReadLine();
                Console.Write("Date of birth: ");
                var dataOfBirth = Console.ReadLine();

                Console.Write("Gender: ");
                var gender = Convert.ToChar(Console.ReadLine());
                Console.Write("Expirience: ");
                var expirience = Convert.ToInt16(Console.ReadLine());
                Console.Write("Account: ");
                var account = Convert.ToDecimal(Console.ReadLine());

                DateTime date;
                CultureInfo iOCultureFormat = new CultureInfo("en-US");
                DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);

                fileCabinetService.EditRecord(id, firstName, lastName, date, gender, expirience, account);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch
            {
                Console.WriteLine("Incorrectly entered data. Repeat entry again");
                Edit(parametrs);
            }
        }

        private static void Find( string parametrs, string property)
        {
            if (parametrs.ToUpper() == "FIRSTNAME")
            {
                var firstName = Console.ReadLine();
                var records = fileCabinetService.FindByFirstName(firstName);

                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
                }
            }

            /* if (property.ToUpper() == "LASTNAME")
             {
                 var records = fileCabinetService.FindByFirstName(parametrs);

                 foreach (var record in records)
                 {
                     Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
                 }
             }

             if (property.ToUpper() == "DATEOFBIRTH")
             {
                 var records = fileCabinetService.FindByFirstName(parametrs);

                 foreach (var record in records)
                 {
                     Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
                 }
             }*/
        }

        private static void List(string parametrs, string filler = "")
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
            }
        }

        private static void Exit(string parameters, string filler = "")
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}