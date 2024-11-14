
using Domain.ValueObjects;

namespace Application
{
    public static class CodeGenerator
    {
        private static string ALPHABET = "!\"%&'()*+,-./0123456789:;<=>?ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";
        public static CodeValue CodeGenerate(int serialPartSize, GTIN gtin, Random rnd)
        {
            char[] alphabet = rnd.GetItems(ALPHABET.AsSpan(), serialPartSize + 4);
            string code = $"010{gtin.Gtin}21{new string(alphabet[..serialPartSize])}\u001d93{new string(alphabet[^4..])}";
            return new CodeValue(code);
        }
        public static async IAsyncEnumerable<CodeValue> GetCodesRange(int serialPartSize, GTIN gtin, Random rnd, int amount, int delayMS)
        {
            for (int i = 0; i < amount; i++)
            {
                await Task.Delay(delayMS);
                yield return CodeGenerate(serialPartSize, gtin, rnd);
            }
        }

        public static CodeValue CodeGenerate(int serialPartSize, Random rnd)
        {
            GTIN gtin = GtinGenerator.GetGTIN(rnd);
            return CodeGenerate(serialPartSize, gtin, rnd);
        }
    }

}
