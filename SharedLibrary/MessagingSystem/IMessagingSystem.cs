
namespace SharedLibrary.MessagingSystem
{
    public interface IMessagingSystem
    {
        void SendMessage<TMessage>(TMessage message);

        void Subscribe<TMessage>(IReceiver receiver);

        void Unsubscribe<TMessage>(IReceiver receiver);
    }
}
