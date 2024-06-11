resource "kubernetes_manifest" "secret_crsecrets" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Secret"
    "metadata" = {
      "name" = "crsecrets"
    }
    "stringData" = {
      "CR_assetManager_Gatewway" = "http://localhost:80/api/asset"
      "CR_authentication_ConnectionString" = "Server=authenticationdb-service; Database=master;User Id=SA;Password=Uxz5#2@1+7"
      "CR_authorization_ConnectionString" = "Server=authorizationdb-service;Database=master;User Id=SA;Password=Uxz5#2@1+7"
      "CR_clamav_Host" = "av-service"
      "CR_jwt_DataEncryptionAlgorithm" = "A256CBC-HS512"
      "CR_jwt_EncryptionKey" = "28472B4B6250655368566D5971337436"
      "CR_jwt_ExpirationInDays" = "30"
      "CR_jwt_Issuer" = "https://github.com/cryoelite"
      "CR_jwt_KeyWrapAlgorithm" = "A256KW"
      "CR_jwt_SigningAlgorithm" = "HS256"
      "CR_jwt_SigningKey" = "a371d63b4dc51bfc77acfbccef6da4d1"
      "CR_kafka_AssetGroup-Primary" = "AssetGroup-Primary"
      "CR_kafka_AssetGroup-Secondary" = "AssetGroup-Secondary"
      "CR_kafka_AssetTopic-Primary" = "AssetTopic-Primary"
      "CR_kafka_AssetTopic-Secondary" = "AssetTopic-Secondary"
      "CR_kafka_AuthGroup-Primary" = "AuthGroup-Primary"
      "CR_kafka_AuthGroup-Secondary" = "AuthGroup-Secondary"
      "CR_kafka_AuthTopic-Primary" = "AuthTopic-Primary"
      "CR_kafka_AuthTopic-Secondary" = "AuthTopic-Secondary"
      "CR_kafka_Broker1" = "broker-service:29092"
      "CR_products_ConnectionString" = "Server=productsdb-service;Database=master;User Id=SA;Password=Uxz5#2@1+7"
    }
    "type" = "Opaque"
  }
}
