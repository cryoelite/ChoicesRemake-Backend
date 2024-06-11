resource "kubernetes_manifest" "deployment_authentication_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "authentication-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "authentication-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "authentication-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "envFrom" = [
                {
                  "secretRef" = {
                    "name" = "crsecrets"
                  }
                },
              ]
              "image" = "localhost:5000/authentication"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "authentication-container"
              "ports" = [
                {
                  "containerPort" = 80
                  "protocol" = "TCP"
                },
              ]
            },
          ]
        }
      }
    }
  }
}
