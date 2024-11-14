
namespace Domain.ValueObjects
{
    public record struct GTIN
    {
        public ulong Gtin { get; init; }
        public GTIN(string gtin)
        {
            if (string.IsNullOrEmpty(gtin) || gtin.Length != 14)
                throw new ArgumentException("gtin has 14 symbols");

            ReadOnlySpan<char> gtinSpan = gtin.AsSpan()[1..];
            Gtin = ulong.Parse(gtinSpan);
        }

        public GTIN(ReadOnlySpan<char> gtin)
        {
            if (gtin.Length != 14)
                throw new ArgumentException("gtin has 14 symbols");
            Gtin = ulong.Parse(gtin[1..]);
        }

        public GTIN(ulong gtin)
        {
            if (gtin > 99_999_999_999_999u)
                throw new ArgumentException("gtin has 14 synbols");
            Gtin = gtin;
        }
        public static GTIN Empty => new GTIN(0UL);

        public override string ToString() => $"{Gtin,14:D14}";
    }

    public static class GtinGenerator
    {
        public static GTIN GetGTIN(Random rnd)
        {
            ulong gtin = (ulong)rnd.NextInt64(1_000_000_000_000, 10_000_000_000_000);
            return new GTIN(gtin);
        }
    }
}
