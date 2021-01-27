using System;

namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    /// <summary>
    /// Class Base command handler.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler commandHandler;

        /// <summary>
        /// Handle the given request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
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
                CommandPromtHandler.GetTheMostSimular(commandRequest.Commands);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Set the next command handler.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <returns>The current command handler.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
            return this.commandHandler;
        }
    }
}