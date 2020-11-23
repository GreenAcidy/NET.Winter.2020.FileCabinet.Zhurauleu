using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators;

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
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("validation", Validation),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "validation", "change type of validation of input data", "The 'help' command change type of validation of input data." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "edit", "modifies existing records", "The 'exit' command modifies existing records." },
            new string[] { "find firstName", "return a list of records with desired firstName.", "The 'find firstName' comand return a list of records with finded firstName." },
            new string[] { "find lastName", "return a list of records with desired lastName.", "The 'find lastName' command return a list of records with finded lastName." },
            new string[] { "find dateofbirth", "return a list of records with desired date of birth.", "The 'find dateOfBirth' comand return a list of records with finded date of birth." },
            new string[] { "export CSV", "export recods in csv format", "The 'export CSV' command exports all records in csv format" },
            new string[] { "export XML", "export recods in xml format", "The 'export XML' command exports all records in xml format" },
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
            Console.WriteLine("Using default validation rules.");
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

        private static void Validation(string parameters)
        {
            do
            {
                if (string.Compare(parameters, "default", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine($"Validation #{parameters} is using now.");
                    break;
                }
                else if (string.Compare(parameters, "custom", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    Console.WriteLine($"Validation #{parameters} is using now.");
                    break;
                }
                else
                {
                    Console.WriteLine($"{parameters} string entered incorrectly, please try again");
                    Console.Write("Validation(default/custom): ");
                    parameters = Console.ReadLine();
                }
            }
            while (isRunning);
        }

        private static void Create(string parametrs)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(StringConverter, FirstNameValidation);

            Console.Write("Last name: ");
            var lastName = ReadInput(StringConverter, LastNameValidation);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var date = ReadInput(DateConverter, DateValidation);

            Console.Write("Gender(M/F): ");
            var gender = ReadInput(CharConverter, GenderValidation);

            Console.Write("Experience: ");
            var experience = ReadInput(ShortConverter, ExperienceValidation);

            Console.Write("Account: ");
            var account = ReadInput(DecimalConverter, AccountValidation);
            FileCabinetInputData inputData = new FileCabinetInputData(firstName, lastName, date, gender, experience, account);
            var index = fileCabinetService.CreateRecord(inputData);
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

            Console.Write("First name: ");
            var firstName = ReadInput(StringConverter, FirstNameValidation);

            Console.Write("Last name: ");
            var lastName = ReadInput(StringConverter, LastNameValidation);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var date = ReadInput(DateConverter, DateValidation);

            Console.Write("Gender: ");
            var gender = ReadInput(CharConverter, GenderValidation);

            Console.Write("Experience: ");
            var experience = ReadInput(ShortConverter, ExperienceValidation);

            Console.Write("Account: ");
            var account = ReadInput(DecimalConverter, AccountValidation);
            FileCabinetInputData inputData = new FileCabinetInputData(firstName, lastName, date, gender, experience, account);

            fileCabinetService.EditRecord(id, inputData);
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

        private static void Export(string parameters)
        {
            string[] property = parameters.Split(' ');
            do
            {
                if (string.Compare(property[0], "csv", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    try
                    {
                        var snapshot = fileCabinetService.MakeSnapShot();
                        using (var streamWriter = new StreamWriter(property[1], false))
                        {
                            snapshot.SaveToCSV(streamWriter);
                            Console.WriteLine($"All record write in file {property[1]}");
                            break;
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Cannot be open this file {property[1]}");
                    }
                }
                else if (string.Compare(property[0], "xml", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    try
                    {
                        var snapshot = fileCabinetService.MakeSnapShot();
                        using (var streamWriter = new StreamWriter(property[1], false))
                        {
                            snapshot.SaveToXML(streamWriter);
                            Console.WriteLine($"All record write in file {property[1]}");
                            break;
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Cannot be open this file {property[1]}");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect property. Please, try again");
                }
            }
            while (isRunning);
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

        private static Tuple<bool, string> FirstNameValidation(string input)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationFirstName(input);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, input);
        }

        private static Tuple<bool, string> LastNameValidation(string input)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationLastName(input);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, input);
        }

        private static Tuple<bool, string> DateValidation(DateTime date)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationData(date);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, date.ToString());
        }

        private static Tuple<bool, string> GenderValidation(char input)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationGender(input);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, input.ToString());
        }

        private static Tuple<bool, string> ExperienceValidation(short input)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationExperience(input);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, input.ToString());
        }

        private static Tuple<bool, string> AccountValidation(decimal input)
        {
            bool flag = true;
            try
            {
                fileCabinetService.Validator.ValidationAccount(input);
            }
            catch
            {
                flag = false;
            }

            return Tuple.Create(flag, input.ToString());
        }

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            bool isConv = true;
            if (input is null)
            {
                isConv = false;
                return Tuple.Create(isConv, input, string.Empty);
            }

            try
            {
                Convert.ChangeType(input, typeof(string));
            }
            catch
            {
                isConv = false;
                return Tuple.Create(isConv, input, string.Empty);
            }

            return Tuple.Create(isConv, input, (string)Convert.ChangeType(input, typeof(string)));
        }

        private static Tuple<bool, string, char> CharConverter(string input)
        {
            bool isConv = true;
            if (input is null)
            {
                isConv = false;
                return Tuple.Create(isConv, input, '1');
            }

            try
            {
                Convert.ChangeType(input, typeof(char));
            }
            catch
            {
                isConv = false;
                return Tuple.Create(isConv, input, '1');
            }

            return Tuple.Create(isConv, input, (char)Convert.ChangeType(input, typeof(char)));
        }

        private static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            bool isConv = true;
            if (input is null)
            {
                isConv = false;
            }

            DateTime date;
            CultureInfo iOCultureFormat = new CultureInfo("en-US");
            DateTime.TryParse(input, iOCultureFormat, DateTimeStyles.None, out date);

            return Tuple.Create(isConv, input, date);
        }

        private static Tuple<bool, string, short> ShortConverter(string input)
        {
            bool isConv = true;
            if (input is null)
            {
                isConv = false;
                return Tuple.Create(isConv, input, (short)1);
            }

            try
            {
                Convert.ChangeType(input, typeof(short));
            }
            catch
            {
                isConv = false;
                return Tuple.Create(isConv, input, (short)1);
            }

            return Tuple.Create(isConv, input, (short)Convert.ChangeType(input, typeof(short)));
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            bool isConv = true;
            if (input is null)
            {
                isConv = false;
                Tuple.Create(isConv, input, 1M);
            }

            try
            {
                Convert.ChangeType(input, typeof(decimal));
            }
            catch
            {
                isConv = false;
                return Tuple.Create(isConv, input, 1M);
            }

            return Tuple.Create(isConv, input, (decimal)Convert.ChangeType(input, typeof(decimal)));
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}