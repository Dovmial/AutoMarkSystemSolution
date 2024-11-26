

using Application.PostOffice;
using Domain.Aggregates.Lines;
using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Domain.Enums;
using Domain.Interfaces;
using SharedLibrary.OperationResult;

namespace Application
{
    public sealed class SessionManager(ISessionDAL dbRepository)
    {
        public SessionEntity? CurrentSession { get; set; }
        
        public event EventHandler<SessionStateEventArgs> SessionStateChanged;

        public async Task<OperationResult<SessionId>> Create(
            Enum_SessionType sessionType,
            ProductId productId,
            ProductionLineId productionLineId,
            DateTime productionDate)
        {
            SessionEntity sessionEntity = new SessionEntity()
            {
                SessionType = sessionType,
                ProductId = productId,
                ProductionLineId = productionLineId,
                ProductionDate = productionDate
            };
            OperationResult<Guid> idResult = await dbRepository.CreateAsync(sessionEntity);
            if (!idResult.IsSuccess)
                return OperationResultCreator.Failure<SessionId>(idResult.Error);
            return OperationResultCreator.SuccessWithValue(new SessionId(idResult.Value));
        }

        public async Task<OperationResult> SetSessionState(Enum_SessionState sessionState)
        {
            bool checkState = sessionState switch
            {
                Enum_SessionState.STARTED => CurrentSession?.State is Enum_SessionState.UNKNOWN or Enum_SessionState.STOPPED,
                Enum_SessionState.STOPPED => CurrentSession?.State is Enum_SessionState.STARTED,
                Enum_SessionState.CLOSED  => CurrentSession?.State is Enum_SessionState.STOPPED,
                Enum_SessionState.SENT    => CurrentSession?.State is Enum_SessionState.CLOSED,
                _ => false
            };
            if (!checkState)
                return OperationResultCreator.Failure(new INVALID_DATA("Incorrect status by session"));
            if (CurrentSession is null)
                return OperationResultCreator.Failure(new INVALID_DATA("Session is null"));
            CurrentSession.State = sessionState;
            if(sessionState is Enum_SessionState.CLOSED)
                CurrentSession.Closed = DateTime.UtcNow;
            var result = await dbRepository.UpdateAsync(CurrentSession);
            SessionStateChanged?.Invoke(this, new(CurrentSession.State));
            return result;
        }
    }
}
