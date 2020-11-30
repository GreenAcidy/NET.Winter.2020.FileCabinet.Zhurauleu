using FileCabinetApp.Interfaces;
using System;

namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected readonly IFileCabinetService fileCabinetService;

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