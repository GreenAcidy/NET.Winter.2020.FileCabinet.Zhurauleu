using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        public const string FindConstant = "find";
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
             : base(fileCabinetService)
        {
            if (printer is null)
            {
                throw new ArgumentNullException($"{nameof(printer)} cannot be null.");
            }

            this.printer = printer;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(FindConstant, commandRequest.Commands, StringComparison.OrdinalIgnoreCase))
            {
                this.Find(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Find(string parameters)
        {
            var findComandAttributes = parameters.Split(new char[] { ' ' });
            switch (findComandAttributes[0].ToUpper())
            {
                case "COMMANDNAME":
                    this.FindCommandName(findComandAttributes[1]);
                    break;
                case "EXECUTIONDATE":
                    this.FindExecutionDate(findComandAttributes[1]);
                    break;
            }
        }

        private void FindCommandName(string commandName)
        {
            var records = fileCabinetService.FindByCommandName(commandName);
            this.printer(records);
        }

        private void FindExecutionDate(string executionDate)
        {
            DateTime date;
            CultureInfo iOCultureFormat = new CultureInfo("en-US");
            DateTime.TryParse(executionDate, iOCultureFormat, DateTimeStyles.None, out date);
            var records = fileCabinetService.FindByExecutionDate(date);

            this.printer(records);
        }
    }
}