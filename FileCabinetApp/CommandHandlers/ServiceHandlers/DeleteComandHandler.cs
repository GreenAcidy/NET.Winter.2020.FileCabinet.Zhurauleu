using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class DeleteComandHandler : ServiceCommandHandlerBase
    {
        public const string DeleteConstant = "delete";
        public const string DeleteKeyWord = "where";

        public DeleteComandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, DeleteConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Delete(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Delete(string parameters)
        {
            var (property, value) = this.Parse(parameters);

            var deletedRecords = this.fileCabinetService.FindBy(property, value);
            var sb = new StringBuilder();

            foreach (var record in deletedRecords)
            {
                sb.Append($"#{record.Id},");
                this.fileCabinetService.Remove(record.Id);
            }

            Console.WriteLine($"Records {sb} are deleted.");
        }

        private (string property, string value) Parse(string parameters)
        {
            if (!parameters.StartsWith(DeleteKeyWord, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{nameof(parameters)} must be start with {nameof(DeleteKeyWord)}");
            }

            parameters = parameters.Substring(DeleteKeyWord.Length);

            var deleteArray = parameters.Split(" = ");

            string property = deleteArray[0].Trim();
            string value = deleteArray[1].Trim('\'', ' ');

            return (property, value);
        }

    }
}
