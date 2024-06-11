resource "kubernetes_manifest" "deployment_rest_proxy_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "rest-proxy-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "rest-proxy-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "rest-proxy-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "env" = [
                {
                  "name" = "KAFKA_REST_BOOTSTRAP_SERVERS"
                  "value" = "broker-service:29092"
                },
                {
                  "name" = "KAFKA_REST_HOST_NAME"
                  "value" = "rest-proxy"
                },
                {
                  "name" = "KAFKA_REST_LISTENERS"
                  "value" = "http://0.0.0.0:8082"
                },
              ]
              "image" = "confluentinc/cp-kafka-rest:6.2.1"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "rest-proxy-container"
              "ports" = [
                {
                  "containerPort" = 8082
                },
              ]
            },
          ]
        }
      }
    }
  }
}
