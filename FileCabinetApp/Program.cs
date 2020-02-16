using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Kiryl Zhurauleu";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
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

        /// <summary>
        /// method connecting the user and the program.
        /// </summary>
        /// <param name="args">input parameter.</param>
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
                    commands[index].Item2(parameters);
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

        private static void PrintHelp(string parameters)
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

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parametrs)
        {
            string firstName, lastName;
            DateTime date;
            char gender;
            short experience;
            decimal account;
            InputData(out firstName, out lastName, out date, out gender, out experience, out account);

            var index = fileCabinetService.CreateRecord(firstName, lastName, date, gender, experience, account);
            Console.WriteLine($"Record #{index} is created.");
        }

        private static void Edit(string parametrs)
        {
            int id = 0;
            bool flag = false;
            do
            {
                try
                {
                    id = int.Parse(parametrs);
                }
                catch (System.FormatException)
                {
                    Console.WriteLine($"Please enter id");
                    id = Convert.ToInt32(Console.ReadLine());
                }

                if (id < 0 || id > fileCabinetService.GetStat())
                {
                    flag = true;
                    Console.WriteLine($"#{id} record is not found");
                }
            }
            while (flag);

            string firstName, lastName;
            DateTime date;
            char gender;
            short experience;
            decimal account;
            InputData(out firstName, out lastName, out date, out gender, out experience, out account);

            fileCabinetService.EditRecord(id, firstName, lastName, date, gender, experience, account);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parametrs)
        {
            Console.WriteLine(parametrs);
            string[] property = parametrs.Split(' ');

            if (string.Compare(property[0], "firstname", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var records = fileCabinetService.FindByFirstName(property[1]);
                foreach (var record in records)
                {
                    Console.WriteLine(record.ToString());
                }
            }
            else if (string.Compare(property[0], "lastname", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var records = fileCabinetService.FindByLastName(property[1]);

                foreach (var record in records)
                {
                    Console.WriteLine(record.ToString());
                }
            }
            else if (string.Compare(property[0], "DayOfBirth", StringComparison.OrdinalIgnoreCase) == 0)
            {
                DateTime date;
                CultureInfo iOCultureFormat = new CultureInfo("en-US");
                DateTime.TryParse(property[1], iOCultureFormat, DateTimeStyles.None, out date);
                var records = fileCabinetService.FindByDateOfBirth(date);

                foreach (var record in records)
                {
                    Console.WriteLine(record.ToString());
                }
            }
            else
            {
                Console.WriteLine("Incorrect property. Please, try again");
            }
        }

        private static void List(string parametrs)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void InputData(out string firstName, out string lastName, out DateTime date, out char gender, out short experience, out decimal account)
        {
            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
                if (ValidationName(firstName))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{firstName} string entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);

            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
                if (ValidationName(lastName))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{lastName} string entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);

            do
            {
                Console.Write("Date of birth (mm/dd/yyyy): ");
                var dataOfBirth = Console.ReadLine();
                CultureInfo iOCultureFormat = new CultureInfo("en-US");
                DateTime.TryParse(dataOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
                if (ValidationData(date))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{date} data entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);

            do
            {
                Console.Write("Gender (M/F): ");
                try
                {
                    gender = Convert.ToChar(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    Console.WriteLine($"Gender must hafe only one symbol!");
                    Console.Write("Gender (M/F): ");
                    gender = Convert.ToChar(Console.ReadLine());
                }

                if (ValidationGender(gender))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{gender} gender entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);

            do
            {
                Console.Write("experience: ");
                experience = Convert.ToInt16(Console.ReadLine());
                if (ValidationExpirience(experience))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{experience} number entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);

            do
            {
                Console.Write("Account: ");
                account = Convert.ToDecimal(Console.ReadLine());
                if (ValidationAccount(account))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{account} number entered incorrectly!");
                    Console.WriteLine();
                }
            }
            while (isRunning);
        }

        private static bool ValidationName(string name)
        {
            if (name is null)
            {
                return false;
            }

            int lenth = name.Length;

            if (lenth < 2 || lenth > 60)
            {
                return false;
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                return false;
            }

            return true;
        }

        private static bool ValidationData(DateTime date)
        {
            var dateNow = DateTime.Today;
            var dateMin = new DateTime(1950, 1, 1);

            if (date.CompareTo(dateMin) < 0)
            {
                return false;
            }

            if (date.CompareTo(dateNow) > 0)
            {
                return false;
            }

            return true;
        }

        private static bool ValidationGender(char gender)
        {
            if (char.IsWhiteSpace(gender))
            {
                return false;
            }

            if (gender == 'm' || gender == 'M' || gender == 'f' || gender == 'F')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ValidationExpirience(short experience)
        {
            if (experience < 0)
            {
                return false;
            }

            return true;
        }

        private static bool ValidationAccount(decimal account)
        {
            if (account <= 0)
            {
                return false;
            }

            return true;
        }
    }
}