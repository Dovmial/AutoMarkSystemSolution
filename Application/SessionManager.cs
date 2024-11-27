

using Application.DTOs;
using Application.Errors;
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
        
        public event EventHandler<SessionStateEventArgs>? SessionStateChanged;
        public event EventHandler<SessionOptionsEventArgs>? SessionOptionsChanged;

        /// <summary>
        /// Создание новой сессии
        /// </summary>
        /// <param name="sessionType"></param>
        /// <param name="productId"></param>
        /// <param name="productionLineId"></param>
        /// <param name="productionDate"></param>
        /// <returns></returns>
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
            //создание сессии
            OperationResult<Guid> idResult = await dbRepository.CreateAsync(sessionEntity);
            if (!idResult.IsSuccess)
                return OperationResultCreator.Failure<SessionId>(idResult.Error);
            OperationResult<SessionId> result = OperationResultCreator.SuccessWithValue(new SessionId(idResult.Value));

            //запрос сессии с продуктом и линией по полученному id
            OperationResult<SessionEntity> newSessionResult = await dbRepository.GetByFullAsync(x => x.Id == result.Value);
            if (!newSessionResult.IsSuccess)
                return OperationResultCreator.Failure<SessionId>(newSessionResult.Error);

            //событие изменения данных сессии
            CurrentSession = newSessionResult.Value;
            SessionOptionsChanged?.Invoke(this, new(
                CurrentSession.Product.Name,
                CurrentSession.ProductionDate.ToString("dd-MM-yyyy"),
                CurrentSession.ProductionLine.Name,
                CurrentSession.State,
                CurrentSession.SessionType));
            return result;
        }

        public async Task<OperationResult> SetSessionState(Enum_SessionState sessionState)
        {
            if (CurrentSession is null)
                return OperationResultCreator.Failure(new ERROR_NULL_SESSION());
            bool checkState = sessionState switch
            {
                Enum_SessionState.STARTED => CurrentSession.State is Enum_SessionState.UNKNOWN or Enum_SessionState.STOPPED,
                Enum_SessionState.STOPPED => CurrentSession.State is Enum_SessionState.STARTED,
                Enum_SessionState.CLOSED  => CurrentSession.State is Enum_SessionState.STOPPED,
                Enum_SessionState.SENT    => CurrentSession.State is Enum_SessionState.CLOSED,
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

            //запуск событий
            SessionStateChanged?.Invoke(this, new(CurrentSession.State));
            
            return result;
        }
    }
}
