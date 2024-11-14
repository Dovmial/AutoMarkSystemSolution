using AmsAPI.Autorize.Models;
using SharedLibrary.OperationResult;

namespace AmsAPI.Autorize.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account, Guid>
    {
        Task<OperationResult<Account>> GetWithRoles(string user);
        Task<OperationResult> Login(Account account);
        Task<OperationResult> LogOut(Account account);
    }
}
