#!/bin/bash
kubectl delete --all pods --namespace=default
kubectl delete --all services --namespace=default
cd configs
kubectl apply -f crsecrets.yml
kubectl apply -f cringress-controller.yml
cd ..
cd services
kubectl apply -f .
cd ..
cd backbone
kubectl apply -f .
cd ..
cd Gateway
kubectl apply -f gateway-deployment.yaml
cd ..
cd CRMicroServices
kubectl apply -f .
cd ..
cd configs
sleep 120s
kubectl apply -f cr-ingress.yml
cd ..
kubectl get pods