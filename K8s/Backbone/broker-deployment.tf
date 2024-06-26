resource "kubernetes_manifest" "deployment_broker_deployment" {
  manifest = {
    "apiVersion" = "apps/v1"
    "kind" = "Deployment"
    "metadata" = {
      "name" = "broker-deployment"
    }
    "spec" = {
      "replicas" = 1
      "selector" = {
        "matchLabels" = {
          "app" = "broker-pod"
        }
      }
      "template" = {
        "metadata" = {
          "labels" = {
            "app" = "broker-pod"
          }
        }
        "spec" = {
          "containers" = [
            {
              "env" = [
                {
                  "name" = "CONFLUENT_METRICS_ENABLE"
                  "value" = "false"
                },
                {
                  "name" = "CONFLUENT_METRICS_REPORTER_BOOTSTRAP_SERVERS"
                  "value" = "broker-service:29092"
                },
                {
                  "name" = "CONFLUENT_METRICS_REPORTER_TOPIC_REPLICAS"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_ADVERTISED_LISTENERS"
                  "value" = "PLAINTEXT://broker-service:29092,PLAINTEXT_HOST://broker-service:9092"
                },
                {
                  "name" = "KAFKA_BROKER_ID"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_CONFLUENT_BALANCER_TOPIC_REPLICATION_FACTOR"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_CONFLUENT_LICENSE_TOPIC_REPLICATION_FACTOR"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS"
                  "value" = "0"
                },
                {
                  "name" = "KAFKA_HEAP_OPTS"
                  "value" = "-Xmx1024M -Xms256M"
                },
                {
                  "name" = "KAFKA_INTER_BROKER_LISTENER_NAME"
                  "value" = "PLAINTEXT"
                },
                {
                  "name" = "KAFKA_LISTENER_SECURITY_PROTOCOL_MAP"
                  "value" = "PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT"
                },
                {
                  "name" = "KAFKA_METRIC_REPORTERS"
                  "value" = "io.confluent.metrics.reporter.ConfluentMetricsReporter"
                },
                {
                  "name" = "KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_TRANSACTION_STATE_LOG_MIN_ISR"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR"
                  "value" = "1"
                },
                {
                  "name" = "KAFKA_ZOOKEEPER_CONNECT"
                  "value" = "zookeeper-service:2181"
                },
              ]
              "image" = "confluentinc/cp-server:6.2.1"
              "imagePullPolicy" = "IfNotPresent"
              "name" = "broker-container"
              "ports" = [
                {
                  "containerPort" = 9092
                },
                {
                  "containerPort" = 9101
                },
                {
                  "containerPort" = 29092
                },
              ]
            },
          ]
        }
      }
    }
  }
}
