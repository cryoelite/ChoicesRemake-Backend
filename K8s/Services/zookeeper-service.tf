resource "kubernetes_manifest" "service_zookeeper_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "zookeeper-service"
    }
    "spec" = {
      "ports" = [
        {
          "name" = "2181"
          "port" = 2181
          "targetPort" = 2181
        },
      ]
      "selector" = {
        "app" = "zookeeper-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
