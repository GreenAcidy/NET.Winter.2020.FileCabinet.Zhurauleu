using System;
using System.IO;
using FileCabinetApp.CommandHandlers.CabinetHandlers;
using FileCabinetApp.CommandHandlers.CommandAgrsHandlers;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.CommandHandlers.ServiceHandlers;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Printers;
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
        private static bool isRunning = true;

        /// <summary>
        /// method connecting the user and the program.
        /// </summary>
        /// <param name="args">input parameter.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            var handler = new Handler();
            handler.Handle(args);
            fileCabinetService = handler.GetService();
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

        public static ICommandHandler CreateCommandHandlers()
        {
            static void Runner(bool x) => isRunning = x;

            var recordPrinter = new DefaultRecordPrinter();
            var helpHandler = new HelpCommandHandler();
            var statHandler = new StatCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, recordPrinter.Print);
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, recordPrinter.Print);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteComandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectHandler = new SelectCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(Runner);
            
            helpHandler.SetNext(statHandler);
            statHandler.SetNext(listHandler);
            listHandler.SetNext(createHandler);
            createHandler.SetNext(importHandler);
            importHandler.SetNext(findHandler);
            findHandler.SetNext(exportHandler);
            exportHandler.SetNext(insertHandler);
            purgeHandler.SetNext(insertHandler);
            insertHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(updateHandler);
            updateHandler.SetNext(selectHandler);
            selectHandler.SetNext(exitHandler);

            return helpHandler;
        }
    }
}