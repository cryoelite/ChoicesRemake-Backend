namespace Authorization.Settings
{
    public class JWTSettings
    {
        public string Issuer { get; set; }

        public string Secret { get; set; }

        public int ExpirationInDays { get; set; }

        public string Algorithm { get; set; }
    }
}