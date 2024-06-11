resource "kubernetes_manifest" "service_authorizationdb_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "authorizationdb-service"
    }
    "spec" = {
      "ports" = [
        {
          "port" = 1433
          "targetPort" = 1433
        },
      ]
      "selector" = {
        "app" = "authorizationdb-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
