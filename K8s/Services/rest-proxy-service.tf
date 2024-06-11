resource "kubernetes_manifest" "service_rest_proxy_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "rest-proxy-service"
    }
    "spec" = {
      "ports" = [
        {
          "name" = "8082"
          "port" = 8082
          "targetPort" = 8082
        },
      ]
      "selector" = {
        "app" = "rest-proxy-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
