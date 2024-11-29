using Domain.Aggregates.MarkingCodes;
using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using SharedLibrary.MessagingSystem;
using SharedLibrary.OperationResult;
using System.Threading.Channels;

namespace Application.LineLogic
{
    internal class CodeHandler(
        IMarkingCodeDAL repository,
        IMessagingSystem messagingSystem,
        SessionQueueManager queueManager,
        ChannelReader<MarkingCodeEntity> codeReader)//?? code или MarkingCode
    {
        private SessionQueueManager sessionQueueManager;
        public async Task Handle(SessionId sessionId, CancellationToken token = default)
        {
            await foreach (var code in codeReader.ReadAllAsync(token))
            {
                code.SessionId = sessionId;
                //code.ParentId = new MarkingCodeId(queueCounter);
               // --queueCounter;
                OperationResult<long> result = await repository.CreateAsync(code);
                if (!result.IsSuccess)
                {
                    messagingSystem.SendMessage(new ErrorMessage(result.Error.ErrorCode));
                }
            }
        }
    }
}
