namespace AmsAPI.Autorize.Models
{
    public record struct AccountId(Guid Value)
    {
        public static AccountId Empty => new AccountId(Guid.Empty);
    }
}