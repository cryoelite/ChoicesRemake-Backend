resource "kubernetes_manifest" "deployment_productsdb_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "productsdb-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "productsdb-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "productsdb-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "image" = "localhost:5000/productsdb"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "productsdb-container"
              "ports" = [
                {
                  "containerPort" = 1433
                },
              ]
            },
          ]
        }
      }
    }
  }
}
