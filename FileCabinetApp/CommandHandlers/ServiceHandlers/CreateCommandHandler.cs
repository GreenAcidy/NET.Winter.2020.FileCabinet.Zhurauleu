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
            var firstName = InputHandler.ReadInput(InputHandler.StringConvrter, InputHandler.FirstNameValidator);

            Console.Write("Last name: ");
            var lastName = InputHandler.ReadInput(InputHandler.StringConvrter, InputHandler.LastNameValidator);

            Console.Write("Date of birth (mm/dd/yyyy): ");
            var dateOfBirth = InputHandler.ReadInput(InputHandler.DateConverter, InputHandler.DateOfBirthValidator);

            Console.Write("Gender(M/F): ");
            var gender = InputHandler.ReadInput(InputHandler.GenderConverter, InputHandler.GenderValidator);

            Console.Write("Experience: ");
            var experience = InputHandler.ReadInput(InputHandler.ExperienceConverter, InputHandler.ExperienceValidator);

            Console.Write("Account: ");
            var account = InputHandler.ReadInput(InputHandler.AccountConverter, InputHandler.AccountValidator);

            var index = this.fileCabinetService.CreateRecord(new FileCabinetInputData(firstName, lastName, dateOfBirth, gender, experience, account));
            Console.WriteLine($"Record #{index} is created.");
        }
    }
}
