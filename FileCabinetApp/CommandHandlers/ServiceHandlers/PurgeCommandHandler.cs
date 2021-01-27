using System;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    /// <summary>
    /// Class Purge command handler.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        private const string PurgeConstant = "purge";

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(commandRequest.Commands, PurgeConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Purge(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Purge(string parameters)
        {
            this.fileCabinetService.Purge();
            Console.WriteLine("Data file processing is completed");
        }
    }
}