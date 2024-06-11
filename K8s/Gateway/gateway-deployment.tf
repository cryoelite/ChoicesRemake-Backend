resource "kubernetes_manifest" "deployment_gateway_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "gateway-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "gateway-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "gateway-pod"
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
              "image" = "localhost:5000/gateway"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "gateway-container"
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
