

namespace Application.PostOffice
{
    //TODO: возможный варинат системы событий
    public class PostOffice
    {
        private Dictionary<Type, List<IReceiver>> _receivers = [];
        public void Register<TMessage>(IReceiver receiver) where TMessage : IMessage
        {
            if(!_receivers.ContainsKey(typeof(TMessage)))
                _receivers[typeof(TMessage)] = new List<IReceiver>();
            var list = _receivers[typeof(TMessage)];
            if(!list.Contains(receiver))
                list.Add(receiver);
        }

        public async Task Post<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (message is null || !_receivers.ContainsKey(message.GetType()))
                return;
            foreach(IReceiver item in _receivers[typeof(TMessage)])
                await item.Handle(message);
        }
    }
}
