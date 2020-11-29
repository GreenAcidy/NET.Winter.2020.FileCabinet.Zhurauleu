
namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    public interface ICommandHandler
    {
        ICommandHandler SetNext(ICommandHandler commandHandler);

        void Handle(AppCommandRequest commandRequest);
    }
}