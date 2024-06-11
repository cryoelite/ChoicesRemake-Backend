resource "kubernetes_manifest" "deployment_av_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "av-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "av-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "av-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "image" = "mkodockx/docker-clamav:alpine"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "av-container"
              "ports" = [
                {
                  "containerPort" = 3310
                },
              ]
            },
          ]
        }
      }
    }
  }
}
