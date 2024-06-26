resource "kubernetes_manifest" "clusterrolebinding_metrics_server_system_auth_delegator" {
  manifest = {
    "apiVersion" = "rbac.authorization.k8s.io/v1beta1"
    "kind" = "ClusterRoleBinding"
    "metadata" = {
      "name" = "metrics-server:system:auth-delegator"
    }
    "roleRef" = {
      "apiGroup" = "rbac.authorization.k8s.io"
      "kind" = "ClusterRole"
      "name" = "system:auth-delegator"
    }
    "subjects" = [
      {
        "kind" = "ServiceAccount"
        "name" = "metrics-server"
        "namespace" = "kube-system"
      },
    ]
  }
}
