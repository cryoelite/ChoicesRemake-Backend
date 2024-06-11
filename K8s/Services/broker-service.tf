resource "kubernetes_manifest" "service_broker_service" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "name" = "broker-service"
    }
    "spec" = {
      "ports" = [
        {
          "name" = "9092"
          "port" = 9092
          "targetPort" = 9092
        },
        {
          "name" = "9101"
          "port" = 9101
          "targetPort" = 9101
        },
        {
          "name" = "29092"
          "port" = 29092
          "targetPort" = 29092
        },
      ]
      "selector" = {
        "app" = "broker-pod"
      }
      "type" = "ClusterIP"
    }
  }
}
