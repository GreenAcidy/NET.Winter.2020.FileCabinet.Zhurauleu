using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    /// <summary>
    /// Class Delete command handler.
    /// </summary>
    public class DeleteComandHandler : ServiceCommandHandlerBase
    {
        private const string DeleteConstant = "delete";
        private const string DeleteKeyWord = "where";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteComandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public DeleteComandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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

            if (string.Equals(property, "id", StringComparison.OrdinalIgnoreCase))
            {
                int id = int.Parse(value, CultureInfo.InvariantCulture);
                this.fileCabinetService.Remove(id);

                Console.WriteLine($"Record #{id} are deleted.");
            }

            var deletedRecords = this.FindRecordForDelete(property, value);

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

        private List<FileCabinetRecord> FindRecordForDelete(string property, string value)
        {
            if (string.Equals(property, "firstName", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByFirstName(value));
            }
            else if (string.Equals(property, "lastName", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByLastName(value));
            }
            else if (string.Equals(property, "dateOfBirth", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByDateOfBirth(Convert.ToDateTime(value)));
            }
            else if (string.Equals(property, "experience", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByExperience(value));
            }
            else if (string.Equals(property, "account", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByAccount(value));
            }
            else if (string.Equals(property, "gender", StringComparison.OrdinalIgnoreCase))
            {
                return new List<FileCabinetRecord>(this.fileCabinetService.FindByGender(value));
            }

            return new List<FileCabinetRecord>();
        }
    }
}
