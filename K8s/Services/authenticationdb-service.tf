resource "kubernetes_manifest" "service_authenticationdb_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "authenticationdb-service"
    }
    "spec" = {
      "ports" = [
        {
          "port" = 1433
          "targetPort" = 1433
        },
      ]
      "selector" = {
        "app" = "authenticationdb-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
