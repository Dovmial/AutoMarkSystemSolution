namespace AmsAPI.Autorize.Interfaces
{
    public interface IPasswordHasher
    {
        (string, string) Hash(string password); //(hash, salt)

        bool Verify(string password, string passwordHash, string salt);
    }
}
