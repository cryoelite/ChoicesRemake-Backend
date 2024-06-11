resource "kubernetes_manifest" "deployment_zookeeper_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "zookeeper-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "zookeeper-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "zookeeper-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "env" = [
                {
                  "name" = "KAFKA_LOG_RETENTION_CHECK_INTERVAL_MS"
                  "value" = "30000"
                },
                {
                  "name" = "KAFKA_LOG_RETENTION_MS"
                  "value" = "60000"
                },
                {
                  "name" = "ZOOKEEPER_CLIENT_PORT"
                  "value" = "2181"
                },
                {
                  "name" = "ZOOKEEPER_TICK_TIME"
                  "value" = "2000"
                },
              ]
              "image" = "confluentinc/cp-zookeeper:6.2.1"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "zookeeper-container"
              "ports" = [
                {
                  "containerPort" = 2181
                },
              ]
            },
          ]
        }
      }
    }
  }
}
