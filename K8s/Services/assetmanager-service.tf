resource "kubernetes_manifest" "service_assetmanager_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "assetmanager-service"
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
        "app" = "assetmanager-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
