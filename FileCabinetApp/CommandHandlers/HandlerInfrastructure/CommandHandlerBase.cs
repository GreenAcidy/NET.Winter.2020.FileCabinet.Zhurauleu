using System;


namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler commandHandler;

        public virtual void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (!(this.commandHandler is null))
            {
                this.commandHandler.Handle(commandRequest);
            }
            else
            {
                Console.WriteLine($"There is no '{commandRequest.Commands}' command.");
                Console.WriteLine();
            }
        }

        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
            return this.commandHandler;
        }
    }
}