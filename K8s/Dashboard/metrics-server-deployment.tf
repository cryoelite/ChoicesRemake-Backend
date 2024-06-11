resource "kubernetes_manifest" "serviceaccount_kube_system_metrics_server" {
  manifest = {
    "apiVersion" = "v1"
    "kind" = "ServiceAccount"
    "metadata" = {
      "name" = "metrics-server"
      "namespace" = "kube-system"
    }
  }
}

resource "kubernetes_manifest" "deployment_kube_system_metrics_server" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "labels" = {
        "k8s-app" = "metrics-server"
      }
      "name" = "metrics-server"
      "namespace" = "kube-system"
    }
    "spec" = {
      "selector" = {
        "matchLabels" = {
          "k8s-app" = "metrics-server"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "k8s-app" = "metrics-server"
          }
          "name" = "metrics-server"
        }
        "spec" = {
          "containers" = [
            {
              "image" = "k8s.gcr.io/metrics-server-amd64:v0.3.6"
              "imagePullPolicy" = "Always"
              "name" = "metrics-server"
              "volumeMounts" = [
                {
                  "mountPath" = "/tmp"
                  "name" = "tmp-dir"
                },
              ]
            },
          ]
          "serviceAccountName" = "metrics-server"
          "volumes" = [
            {
              "emptyDir" = {}
              "name" = "tmp-dir"
            },
          ]
        }
      }
    }
  }
}
