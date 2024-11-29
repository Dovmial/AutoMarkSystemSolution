

namespace Application.Producers
{
    public interface ICodeSource
    {
        Task<string> GetCodeAsync(CancellationToken token);
        Task<ICollection<string>> GetRangeCodesAsync(CancellationToken token);
    }
}
