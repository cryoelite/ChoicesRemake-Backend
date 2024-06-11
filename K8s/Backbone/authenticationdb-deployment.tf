resource "kubernetes_manifest" "deployment_authenticationdb_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "authenticationdb-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "authenticationdb-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "authenticationdb-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "image" = "localhost:5000/authenticationdb"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "authenticationdb-container"
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
