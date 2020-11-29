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
                case "FIRSTNAME":
                    this.FindFirstName(findComandAttributes[1]);
                    break;
                case "LASTNAME":
                    this.FindLastName(findComandAttributes[1]);
                    break;
                case "DATEOFBIRTH":
                    this.FindDateOfBirth(findComandAttributes[1]);
                    break;
            }
        }

        private void FindFirstName(string firstName)
        {
            var records = fileCabinetService.FindByFirstName(firstName);
            this.printer(records);
        }

        private void FindLastName(string lastName)
        {
            var records = fileCabinetService.FindByLastName(lastName);
            this.printer(records);
        }

        private void FindDateOfBirth(string dateOfBirth)
        {
            DateTime date;
            CultureInfo iOCultureFormat = new CultureInfo("en-US");
            DateTime.TryParse(dateOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
            var records = fileCabinetService.FindByDateOfBirth(date);

            this.printer(records);
        }
    }
}