resource "kubernetes_manifest" "deployment_authorization_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "authorization-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "authorization-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "authorization-pod"
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
              "image" = "localhost:5000/authorization"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "authorization-container"
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
