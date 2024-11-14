namespace AmsAPI.Autorize.Models
{
    public record struct RoleId(int Value)
    {
        public static Role Empty => new();
    }
}
