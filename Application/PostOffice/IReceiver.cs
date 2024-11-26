
namespace Application.PostOffice
{
    public interface IReceiver
    {
        Task Handle(IMessage message);
    }
}
