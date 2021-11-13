namespace StaticAssets
{
    public static class ConfigurationKeys
    {
        public const string connectionString = "connectionString";

        #region authentication

        public const string authentication_jwt = "Jwt";
        public const string authentication_jwt_DataEncryptionAlgorithm = "DataEncryptionAlgorithm";
        public const string authentication_jwt_EncryptionKey = "EncryptionKey";
        public const string authentication_jwt_expiration = "ExpirationInDays";
        public const string authentication_jwt_issuer = "Issuer";
        public const string authentication_jwt_KeyWrapAlgorithm = "KeyWrapAlgorithm";
        public const string authentication_jwt_SigningAlgorithm = "SigningAlgorithm";
        public const string authentication_jwt_SigningKey = "SigningKey";
        public const string authenticationSection = "Authentication";

        #endregion authentication

        #region products

        public const string productsSection = "Products";

        #endregion products

        #region authorization

        public const string authorizationSection = "Authorization";

        #endregion authorization

        #region kafka

        public const string kafka_authGroupPrimary = "authgroup-Primary";
        public const string kafka_authGroupSecondary = "authgroup-Secondary";
        public const string kafka_authTopicPrimary = "authTopic-Primary";
        public const string kafka_authTopicSecondary = "authTopic-Secondary";
        public const string kafka_broker1 = "broker1";
        public const string kafkaSection = "Kafka";

        #endregion kafka
    }
}