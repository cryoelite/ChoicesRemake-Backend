resource "kubernetes_manifest" "service_kube_system_metrics_server" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "Service"
    "metadata" = {
      "labels" = {
        "kubernetes.io/cluster-service" = "true"
        "kubernetes.io/name" = "Metrics-server"
      }
      "name" = "metrics-server"
      "namespace" = "kube-system"
    }
    "spec" = {
      "ports" = [
        {
          "port" = 443
          "protocol" = "TCP"
          "targetPort" = 443
        },
      ]
      "selector" = {
        "k8s-app" = "metrics-server"
      }
    }
  }
}
