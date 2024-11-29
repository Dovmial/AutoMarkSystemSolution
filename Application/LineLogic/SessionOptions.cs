using Domain.ValueObjects;

namespace Application.LineLogic
{
    internal record class SessionOptions(int CodeSerialLength, GTIN GTIN);
}
