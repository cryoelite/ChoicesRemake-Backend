resource "kubernetes_manifest" "rolebinding_kube_system_metrics_server_auth_reader" {
  manifest = {
    "apiVersion" = "rbac.authorization.k8s.io/v1beta1"
    "kind" = "RoleBinding"
    "metadata" = {
      "name" = "metrics-server-auth-reader"
      "namespace" = "kube-system"
    }
    "roleRef" = {
      "apiGroup" = "rbac.authorization.k8s.io"
      "kind" = "Role"
      "name" = "extension-apiserver-authentication-reader"
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
