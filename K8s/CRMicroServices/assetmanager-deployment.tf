resource "kubernetes_manifest" "deployment_assetmanager_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "assetmanager-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "assetmanager-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "assetmanager-pod"
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
              "image" = "localhost:5000/asset"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "assetmanager-container"
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
