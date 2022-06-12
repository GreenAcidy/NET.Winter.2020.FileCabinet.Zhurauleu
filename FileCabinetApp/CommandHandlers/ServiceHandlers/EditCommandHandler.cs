using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        public const string EditConstant = "edit";

        public EditCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, EditConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Edit(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Edit(string parameters)
        {
            var id = int.Parse(parameters);
            if (id > this.fileCabinetService.GetStat().real)
            {
                Console.WriteLine($"#{id} records is not found.");
            }
            else
            {
                Console.Write("Command name: ");
                var commandName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.commandNameValidator);

                Console.Write("Execution date (mm/dd/yyyy): ");
                var executionDate = InputValidator.ReadInput(InputValidator.dateConverter, InputValidator.executionDateValidator);

                Console.Write("Experience: ");
                var experience = InputValidator.ReadInput(InputValidator.experienceConverter, InputValidator.experienceValidator);

                var index = this.fileCabinetService.CreateRecord(new FileCabinetInputData(commandName, executionDate, experience));
                Console.WriteLine($"Record #{id} is edited.");
            }
        }
    }
}
