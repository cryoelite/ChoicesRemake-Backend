resource "kubernetes_manifest" "service_authorization_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "authorization-service"
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
        "app" = "authorization-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
