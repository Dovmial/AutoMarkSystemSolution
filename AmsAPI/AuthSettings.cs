namespace AmsAPI
{
    public record class AuthSettings(TimeSpan Expires, string SecretKey, string CookieName)
    {
        public AuthSettings() : this(TimeSpan.MinValue, string.Empty, string.Empty)
        {}
    }
}
