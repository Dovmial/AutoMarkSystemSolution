

namespace Application.Producers
{
    public interface ISourceCode
    {
        Task<string> GetCodeAsync(CancellationToken token);
        Task<ICollection<string>> GetRangeCodesAsync(CancellationToken token);
    }
}
