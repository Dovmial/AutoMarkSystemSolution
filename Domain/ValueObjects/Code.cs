

using SharedLibrary.OperationResult;

namespace Domain.ValueObjects
{
    public record class CodeValue(string Code)
    {
        public static CodeValue Empty => new CodeValue(string.Empty);
    }

    public static class Code
    {
        public static GTIN GetGtin(CodeValue code) => new GTIN(code.Code.AsSpan()[2..16]);
        public static GTIN GetGtin(string gtin) => new GTIN(gtin);
        public static GTIN GetGTIN(ReadOnlySpan<char> gtin) => new GTIN(gtin);
        public static OperationResult CheckStruct(string codeUtf8, int serialLength, GTIN gtin, bool hasCryptoKey = true)
        {
            //01{gtin,14}21{serialNumber}{\u001d}93{cryptoKey}
            ReadOnlySpan<char> codeSpan = codeUtf8.AsSpan();

            //проверка соответсnвия gtin
            OperationResult result = CheckGTIN(gtin, codeSpan);
            if (result.IsSuccess == false)
                return result;

            if (hasCryptoKey) {
            //проверка криптоключа
                int code29Index = codeSpan.IndexOf('\u001d');
                result = CheckCryptoKey(code29Index, codeSpan);
                if (result.IsSuccess == false)
                    return result;
            //проверка серийного номера
                result = CheckSerialNumber(serialLength, hasCryptoKey, codeSpan, code29Index);
                return result;
            }
            //проверка серийного номера, когда нет криптоключа
            return CheckSerialNumber(serialLength, hasCryptoKey, codeSpan);
        }

        #region private
        private static OperationResult CheckCryptoKey(int code29Index, ReadOnlySpan<char> codeSpan)
        {
            if (code29Index == -1 || codeSpan[code29Index..].Length != 7)
                return OperationResultCreator.Failure(new CRYPTOKEY_ERROR());
            return OperationResultCreator.Success;
        }

        private static OperationResult CheckSerialNumber(int serialLength, bool hasCryptoKey, ReadOnlySpan<char> codeSpan, int code29Index = -1)
        {
            if (hasCryptoKey)
            {
                if (serialLength != code29Index - 18)
                    return OperationResultCreator.Failure(new SERIAL_NUMBER_LENGTH_ERROR());
            }
            else if (codeSpan[18..].Length != serialLength)
                return OperationResultCreator.Failure(new SERIAL_NUMBER_LENGTH_ERROR());
            return OperationResultCreator.Success;
        }

        private static OperationResult CheckGTIN(GTIN gtin, ReadOnlySpan<char> codeSpan)
        {
            if (ulong.TryParse(codeSpan[2..16], out ulong gtinNumber))
            {
                if (gtin.Gtin != gtinNumber)
                    return OperationResultCreator.Failure(new NOT_VALID_GTIN_ERROR());
            }
            else
                return OperationResultCreator.Failure(new NOT_VALID_GTIN_ERROR());
            return OperationResultCreator.Success;
        }
        #endregion
    }
}
