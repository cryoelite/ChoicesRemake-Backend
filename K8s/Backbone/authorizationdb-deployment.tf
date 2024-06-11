resource "kubernetes_manifest" "deployment_authorizationdb_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "authorizationdb-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "authorizationdb-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "authorizationdb-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "image" = "localhost:5000/authorizationdb"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "authorizationdb-container"
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
