using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;

namespace FileCabinetApp.CommandHandlers.CabinetHandlers
{
    /// <summary>
    /// Help command handler.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const string HelpConstant = "help";

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'stat' command show statistics by records." },
            new string[] { "create", "receive user input and and create new record.", "The 'create' command receive user input and create new record." },
            new string[] { "update", "modifies existing records", "The 'update' command modifies existing records." },
            new string[] { "insert", "add new record using received data", "The 'update' command add new record using received data." },
            new string[] { "export CSV", "export recods in csv format", "The 'export CSV' command exports all records in csv format" },
            new string[] { "export XML", "export recods in xml format", "The 'export XML' command exports all records in xml format" },
            new string[] { "import CSV", "import records from csv file.", "The 'import CSV' command import all records from csv file." },
            new string[] { "import XML", "import records from xml file.", "The 'import XML' command import all records from xml file." },
            new string[] { "delete", "delete record from the service.", "The 'delete' command delete record from the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, HelpConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Help(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Help(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
