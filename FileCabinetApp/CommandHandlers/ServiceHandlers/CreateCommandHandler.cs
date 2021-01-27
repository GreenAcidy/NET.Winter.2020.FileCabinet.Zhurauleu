using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    /// <summary>
    /// Class Create command handler.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const string CreateConstant = "create";

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(commandRequest.Commands, CreateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Create(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Create(string parametrs)
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
            Console.WriteLine($"Record #{index} is created.");
        }
    }
}
