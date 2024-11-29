
using Application.Producers;
using Domain.ValueObjects;

namespace Application
{
    public class CodeGenerator(GTIN gtin, int serialPartSize, int packSize): ISourceCode
    {
        private readonly int _delayMs = 250;
        private const string ALPHABET = "!\"%&'()*+,-./0123456789:;<=>?ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";
        private async Task<string> CodeGenerate(CancellationToken token)
        {
            await Task.Delay(_delayMs, token);
            char[] alphabet = Random.Shared.GetItems(ALPHABET.AsSpan(), serialPartSize + 4);
            string code = $"010{gtin.Gtin}21{new string(alphabet[..serialPartSize])}\u001d93{new string(alphabet[^4..])}";
            return code;
        }
        private async Task<ICollection<string>> GetCodesRange(CancellationToken token)
        {
            await Task.Delay(_delayMs, token);
            ICollection<string> codes = [];
            for(int i = 0; i < packSize; i++)
                codes.Add(await CodeGenerate(token));
            return codes;
        }

        public async Task<string> GetCodeAsync(CancellationToken token)
        {
            return await CodeGenerate(token);
        }

        public async Task<ICollection<string>> GetRangeCodesAsync(CancellationToken token)
        {
            return await GetCodesRange(token);
        }
    }

}
