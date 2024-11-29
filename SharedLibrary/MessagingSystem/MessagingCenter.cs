

namespace SharedLibrary.MessagingSystem
{
    public class MessagingCenter : IMessagingSystem
    {
        private Dictionary<Type, List<IReceiver>> _subscribers = [];

        public void SendMessage<TMessage>(TMessage message)
        {
            Type typeMessage = typeof(TMessage);
            if (_subscribers.ContainsKey(typeMessage))
            {
                foreach (var subscriber in _subscribers[typeMessage])
                    subscriber.HandleMessage(message);
            }
        }

        public void Subscribe<TMessage>(IReceiver receiver)
        {
            Type key = typeof(TMessage);
            if (!_subscribers.ContainsKey(key))
                _subscribers[key] = new List<IReceiver>();
            _subscribers[key].Add(receiver);
        }

        public void Unsubscribe<TMessage>(IReceiver receiver)
        {
            Type key = typeof(TMessage);
            if (_subscribers.ContainsKey(key))
            {
                _subscribers[key].Remove(receiver);
                if (_subscribers[key].Count == 0)
                    _subscribers.Remove(key);
            }
        }
    }
}
