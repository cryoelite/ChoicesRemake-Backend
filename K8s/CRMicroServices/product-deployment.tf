resource "kubernetes_manifest" "deployment_product_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "product-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "product-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "product-pod"
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
              "image" = "localhost:5000/product"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "product-container"
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
