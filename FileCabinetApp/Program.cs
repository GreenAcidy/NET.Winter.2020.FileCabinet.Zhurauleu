using System;
using System.IO;
using FileCabinetApp.CommandHandlers.CabinetHandlers;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.CommandHandlers.ServiceHandlers;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Service;
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
        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator validator;
        private static bool isDefaultRule;
        private static bool isRunning = true;

        private static string[] commandLineParameters = new string[]
        {
            "--validation-rules",
            "-v",
            "--storage",
            "-s",
        };

        /// <summary>
        /// method connecting the user and the program.
        /// </summary>
        /// <param name="args">input parameter.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            CommandAgrsHandler(args);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];
                var commandHandlers = CreateCommandHandlers();

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                const int parametersIndex = 1;
                string parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandlers.Handle(new AppCommandRequest(command, parameters));
                Console.WriteLine();
            }
            while (isRunning);
        }

        private static void CommandAgrsHandler(string[] args)
        {
            string rule;
            int commandIndex = ParseRule(args, out rule);
            switch (rule)
            {
                case "DEFAULT":
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    isDefaultRule = true;
                    validator = new DefaultValidator();
                    break;
                case "CUSTOM":
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    Console.WriteLine("Using custom validation rules.");
                    validator = new CustomValidator();
                    break;
                default:
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    isDefaultRule = true;
                    validator = new DefaultValidator();
                    break;
            }

            if (commandIndex >= 3)
            {
                switch (rule)
                {
                    case "MEMORY":
                        fileCabinetService = new FileCabinetMemoryService(validator);
                        Console.WriteLine("Use memory service");
                        break;
                    case "FILE":
                        string fullPath = "cabinet-records.db";
                        FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream);
                        Console.WriteLine("Use file service");
                        break;
                }
            }
        }

        private static int ParseRule(string[] args, out string rule)
        {
            if (args.Length == 0)
            {
                rule = string.Empty;
                return -1;
            }

            int index = -1;

            var parseText = args[0].Split(' ');
            if (parseText[0] == commandLineParameters[0])
            {
                rule = parseText[parseText.Length - 1].ToUpper();
                index = 0;
            }
            else if (parseText[0] == commandLineParameters[1])
            {
                rule = args[1].ToUpper();
                index = 1;
            }
            else if (parseText[0] == commandLineParameters[2])
            {
                rule = parseText[parseText.Length - 1].ToUpper();
                index = 3;
            }
            else if (parseText[0] == commandLineParameters[3])
            {
                rule = args[1].ToUpper();
                index = 4;
            }
            else
            {
                rule = string.Empty;
            }

            return index;
        }

        public static ICommandHandler CreateCommandHandlers()
        {
            static void Runner(bool x) => isRunning = x;

            var helpHandler = new HelpCommandHandler();
            var statHandler = new StatCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(Runner);
            var editHandler = new EditCommandHandler(fileCabinetService);

            helpHandler.SetNext(statHandler);
            statHandler.SetNext(listHandler);
            listHandler.SetNext(createHandler);
            createHandler.SetNext(importHandler);
            importHandler.SetNext(findHandler);
            findHandler.SetNext(exportHandler);
            exportHandler.SetNext(removeHandler);
            removeHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);
            exitHandler.SetNext(editHandler);

            return helpHandler;
        }
    }
}