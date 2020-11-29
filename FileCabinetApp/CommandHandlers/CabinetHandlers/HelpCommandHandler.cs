using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using System;

namespace FileCabinetApp.CommandHandlers.CabinetHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        public const string HelpConstant = "help";

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "show statistics by records.", "The 'create' command show statistics by records." },
            new string[] { "create", "receive user input and and create new record.", "The 'exit' command receive user input and create new record." },
            new string[] { "edit", "modifies existing records", "The 'exit' command modifies existing records." },
            new string[] { "find firstName", "return a list of records with desired firstName.", "The 'find firstName' comand return a list of records with finded firstName." },
            new string[] { "find lastName", "return a list of records with desired lastName.", "The 'find lastName' command return a list of records with finded lastName." },
            new string[] { "find dateofbirth", "return a list of records with desired date of birth.", "The 'find dateOfBirth' comand return a list of records with finded date of birth." },
            new string[] { "export CSV", "export recods in csv format", "The 'export CSV' command exports all records in csv format" },
            new string[] { "export XML", "export recods in xml format", "The 'export XML' command exports all records in xml format" },
            new string[] { "import CSV", "import records from csv file.", "The 'import CSV' command import all records from csv file." },
            new string[] { "import XML", "import records from xml file.", "The 'import XML' command import all records from xml file." },
            new string[] { "remove", "remove record from the service.", "The 'remove' command remove record from the service." },
            new string[] { "list", "return a list of records added to the service.", "The 'exit' command return a list of records added to the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

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
