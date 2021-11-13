namespace StaticAssets
{
    public class JWTSettings
    {
        public string DataEncryptionAlgorithm { get; set; } = string.Empty;
        public string EncryptionKey { get; set; } = string.Empty;
        public int ExpirationInDays { get; set; } = 0;
        public string Issuer { get; set; } = string.Empty;
        public string KeyWrapAlgorithm { get; set; } = string.Empty;
        public string SigningAlgorithm { get; set; } = string.Empty;
        public string SigningKey { get; set; } = string.Empty;
    }
}