

namespace SharedLibrary.MessagingSystem
{
    public interface IReceiver
    {
        void HandleMessage<TMessage>(TMessage message);
    }
}
