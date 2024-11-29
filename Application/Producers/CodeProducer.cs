

using Application.LineLogic;
using Domain.ValueObjects;
using SharedLibrary.MessagingSystem;
using SharedLibrary.OperationResult;
using System.Threading.Channels;

namespace Application.Producers
{
    /// <summary>
    /// 1. Принимает коды из источника
    /// 2. Проверяет правильность структуры
    /// 3. Хорошие отправляет по каналу дальше в систему
    /// 4. Плохие не отбрасываются, генерируется сообщение об ошибки через систему сообщений. 
    /// 5. Если приходит группа кодов и среди них хотя бы один плохой - отбрасывается вся группа
    /// </summary>
    internal class CodeProducer
    {
        internal ChannelReader<CodeValue> Reader { get; init; }
        private readonly IMessagingSystem _messagingSystem;
        private readonly Channel<CodeValue> _channelCode;
        private readonly ISourceCode _codeSource;
        private readonly SessionOptions _sessionOptions;
        private readonly CancellationToken _token;
        public CodeProducer(
            IMessagingSystem messagingSystem,
            ISourceCode codeSource,
            SessionOptions sessionOptions,
            CancellationToken token = default)
        {
            _codeSource = codeSource;
            _sessionOptions = sessionOptions;
            _messagingSystem = messagingSystem;
            _token = token;

            _channelCode = Channel.CreateUnbounded<CodeValue>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true,
            });
            Reader = _channelCode.Reader;
        }

        public async Task<OperationResult> Start(Enum_StrategyTypeHandle strategyType)
        {
            Func<Task> strategy = strategyType switch
            {
                Enum_StrategyTypeHandle.SINGLE => SendSingleCode,
                Enum_StrategyTypeHandle.RANGE => SendRangeCodes,
                _ => throw new NotSupportedException()
            };
            OperationResult? result = null;
            try
            {
                //Запуск приема одиночных кодов из источника
                while (!_token.IsCancellationRequested)
                    await strategy();
                
                result = OperationResultCreator.Success;
            }
            catch (OperationCanceledException ex)
            {
                result = OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
            catch (Exception ex)
            {
                result = OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
            return result;
        }

        private async Task SendSingleCode()
        {
            string codeStr = await _codeSource.GetCodeAsync(_token);
            if (!CheckStruct(codeStr).IsSuccess)
                return;
            await HandleSingleCode(codeStr);
        }

        private async Task SendRangeCodes()
        {
            ICollection<string> codesStr = await _codeSource.GetRangeCodesAsync(_token);
            foreach (string codeStr in codesStr)
                if(!CheckStruct(codeStr).IsSuccess)
                    return;
            foreach (string codeStr in codesStr)
                await HandleSingleCode(codeStr);
        }

        private async Task HandleSingleCode(string codeStr)
        {
            //отправка кода обработчикам
            CodeValue code = new(codeStr);
            await _channelCode.Writer.WriteAsync(code, _token);
        }

        private OperationResult CheckStruct(string codeStr)
        {
            OperationResult checkResult = Code.CheckStruct(codeStr, _sessionOptions.CodeSerialLength, _sessionOptions.GTIN);
            //обработка ошибки структуры
            if (!checkResult.IsSuccess)
                _messagingSystem.SendMessage(new ErrorMessage($"Ошибка: {checkResult.Error.ErrorCode}"));
            return checkResult;
        }
    }
}
