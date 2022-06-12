using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public const string CreateConstant = "create";

        public CreateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, CreateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Create(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        public void Create(string parametrs)
        {
            Console.Write("Command name: ");
            var commandName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.commandNameValidator);

            Console.Write("Execution date (mm/dd/yyyy): ");
            var executionDate = InputValidator.ReadInput(InputValidator.dateConverter, InputValidator.executionDateValidator);

            Console.Write("Experience: ");
            var experience = InputValidator.ReadInput(InputValidator.experienceConverter, InputValidator.experienceValidator);

            var index = this.fileCabinetService.CreateRecord(new FileCabinetInputData(commandName, executionDate, experience));
            Console.WriteLine($"Record #{index} is created.");
        }
    }
}
