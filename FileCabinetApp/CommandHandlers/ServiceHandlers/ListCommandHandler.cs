using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public const string ListConstant = "list";

        public ListCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, ListConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.List(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void List(string parametrs)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToLongDateString()}, {record.Gender}, {record.Experience}, {record.Account}");
            }
        }
    }
}