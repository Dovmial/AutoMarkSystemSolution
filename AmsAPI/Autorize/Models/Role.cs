namespace AmsAPI.Autorize.Models
{
    public class Role(string role)
    {
        public RoleId Id { get; set; }
        public string Name { get; set; } = role;
        public ICollection<Account> Accounts { get; set; } = [];

        public Role(): this(string .Empty)
        { 
        }
    }
}
