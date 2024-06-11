resource "kubernetes_manifest" "clusterrole_system_metrics_server" {
  manifest = {
    "apiVersion" = "rbac.authorization.k8s.io/v1"
    "kind" = "ClusterRole"
    "metadata" = {
      "name" = "system:metrics-server"
    }
    "rules" = [
      {
        "apiGroups" = [
          "",
        ]
        "resources" = [
          "pods",
          "nodes",
          "nodes/stats",
          "namespaces",
        ]
        "verbs" = [
          "get",
          "list",
          "watch",
        ]
      },
    ]
  }
}

resource "kubernetes_manifest" "clusterrolebinding_system_metrics_server" {
  manifest = {
    "apiVersion" = "rbac.authorization.k8s.io/v1"
    "kind" = "ClusterRoleBinding"
    "metadata" = {
      "name" = "system:metrics-server"
    }
    "roleRef" = {
      "apiGroup" = "rbac.authorization.k8s.io"
      "kind" = "ClusterRole"
      "name" = "system:metrics-server"
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
