

using Domain.Aggregates.Sessions;
using Domain.Enums;

namespace Application.LineLogic
{
    internal class SessionQueueManager
    {
        private readonly List<int> countersQueue;
        public SessionQueueManager(Enum_SessionType sessionType)
        {
            int size = (int)sessionType;
            countersQueue = new(size);
            for (int i = 0; i < size; i++)
                countersQueue[i] = -1;
        }

        public int GetCounter(int idx) => countersQueue[idx];
        public void SetCounter(int idx, int value) => countersQueue[idx] = value;


    }
}
