using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    /// <summary>
    /// Base command hanlder for services command.
    /// </summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// The current service.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected readonly IFileCabinetService fileCabinetService;
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            if (fileCabinetService is null)
            {
                throw new ArgumentNullException($"{nameof(fileCabinetService)} cannot be null.");
            }

            this.fileCabinetService = fileCabinetService;
        }
    }
}