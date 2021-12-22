#!/bin/bash
/usr/local/bin/docker-compose build
docker run -d -p 5000:5000 --restart=always --name registry registry:2
sleep 10s
docker push localhost:5000/product
docker push localhost:5000/gateway
docker push localhost:5000/authorization
docker push localhost:5000/authentication
docker push localhost:5000/asset
docker push localhost:5000/authenticationdb
docker push localhost:5000/authorizationdb
docker push localhost:5000/productsdb