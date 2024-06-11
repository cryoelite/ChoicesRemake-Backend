resource "kubernetes_manifest" "service_authentication_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "authentication-service"
    }
    "spec" = {
      "ports" = [
        {
          "port" = 80
          "protocol" = "TCP"
          "targetPort" = 80
        },
      ]
      "selector" = {
        "app" = "authentication-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
