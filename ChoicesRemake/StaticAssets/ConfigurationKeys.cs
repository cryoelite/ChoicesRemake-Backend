namespace StaticAssets
{
    public static class ConfigurationKeys
    {

        #region authentication

        public const string authentication_connectionString = "CR_authentication_ConnectionString";
        public const string authentication_jwt_DataEncryptionAlgorithm = "CR_jwt_DataEncryptionAlgorithm";
        public const string authentication_jwt_EncryptionKey = "CR_jwt_EncryptionKey";
        public const string authentication_jwt_expiration = "CR_jwt_ExpirationInDays";
        public const string authentication_jwt_issuer = "CR_jwt_Issuer";
        public const string authentication_jwt_KeyWrapAlgorithm = "CR_jwt_KeyWrapAlgorithm";
        public const string authentication_jwt_SigningAlgorithm = "CR_jwt_SigningAlgorithm";
        public const string authentication_jwt_SigningKey = "CR_jwt_SigningKey";


        #endregion authentication

        #region products

        public const string products_connectionString = "CR_products_ConnectionString";

        #endregion products

        #region authorization

        public const string authorization_connectionString = "CR_authorization_ConnectionString";

        #endregion authorization

        #region kafka

        public const string kafka_assetGroupPrimary = "CR_kafka_AssetGroup-Primary";
        public const string kafka_assetGroupSecondary = "CR_kafka_AssetGroup-Secondary";
        public const string kafka_assetTopicPrimary = "CR_kafka_AssetTopic-Primary";
        public const string kafka_assetTopicSecondary = "CR_kafka_AssetTopic-Secondary";
        public const string kafka_authGroupPrimary = "CR_kafka_AuthGroup-Primary";
        public const string kafka_authGroupSecondary = "CR_kafka_AuthGroup-Secondary";
        public const string kafka_authTopicPrimary = "CR_kafka_AuthTopic-Primary";
        public const string kafka_authTopicSecondary = "CR_kafka_AuthTopic-Secondary";
        public const string kafka_broker1 = "CR_kafka_Broker1";

        #endregion kafka

        #region assetManager

        public const string assetManager_Gateway = "CR_assetManager_Gatewway";

        #endregion assetManager

        #region clamAV

        public const string clamAV_Host = "CR_clamav_Host";

        #endregion clamAV
    }
}