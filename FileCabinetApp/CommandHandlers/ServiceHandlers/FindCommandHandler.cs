using System;
using System.Globalization;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        public const string FindConstant = "find";

        public FindCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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
            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private void FindLastName(string lastName)
        {
            var records = fileCabinetService.FindByLastName(lastName);
            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private void FindDateOfBirth(string dateOfBirth)
        {
            DateTime date;
            CultureInfo iOCultureFormat = new CultureInfo("en-US");
            DateTime.TryParse(dateOfBirth, iOCultureFormat, DateTimeStyles.None, out date);
            var records = fileCabinetService.FindByDateOfBirth(date);

            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}