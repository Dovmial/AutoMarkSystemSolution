using Microsoft.AspNetCore.Identity.Data;
using System.Net;

namespace AmsAPI.Autorize.Models
{
    public class Account(string login, string pass)
    {
        public AccountId Id { get; set; }
        public string Login { get; set; } = login;
        public string HashPass { get; set; } = pass;
        public string Salt { get; set; } = null!;
        public DateTime DateOfRegistration { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public bool IsOnline { get; set; }
        public string? IpAddress { get; set; }
        public ICollection<Role> Roles { get; set; } = [];

        public static Account Empty => new Account(string.Empty, string.Empty);
        public bool IsEmpty => string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(HashPass);

        public Account() : this(string.Empty, string.Empty) { }
    }
}
