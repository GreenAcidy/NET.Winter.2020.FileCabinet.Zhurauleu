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
            if (id > this.fileCabinetService.GetStat().active)
            {
                Console.WriteLine($"#{id} records is not found.");
            }
            else
            {
                Console.Write("First name: ");
                var firstName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.firstNameValidator);

                Console.Write("Last name: ");
                var lastName = InputValidator.ReadInput(InputValidator.stringConvrter, InputValidator.lastNameValidator);

                Console.Write("Date of birth (mm/dd/yyyy): ");
                var dateOfBirth = InputValidator.ReadInput(InputValidator.dateConverter, InputValidator.dateOfBirthValidator);

                Console.Write("Gender(M/F): ");
                var gender = InputValidator.ReadInput(InputValidator.genderConverter, InputValidator.genderValidator);

                Console.Write("Experience: ");
                var experience = InputValidator.ReadInput(InputValidator.experienceConverter, InputValidator.experienceValidator);

                Console.Write("Account: ");
                var account = InputValidator.ReadInput(InputValidator.accountConverter, InputValidator.accountValidator);

                var index = this.fileCabinetService.CreateRecord(new FileCabinetInputData(firstName, lastName, dateOfBirth, gender, experience, account));
                Console.WriteLine($"Record #{id} is edited.");
            }
        }
    }
}
