server

```
docker build -t agones-udp-server-magiconionchatapp:0.1.1 -f ChatApp.Server/Dockerfile .
docker tag agones-udp-server-magiconionchatapp:0.1.1 guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
docker push guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
```


match

```
docker build -t magiconionchatapp-match:0.0.1 -f ChatApp.Match/Dockerfile .
docker tag magiconionchatapp-match:0.0.1 guitarrapc/magiconionchatapp-match:0.0.1
docker push guitarrapc/magiconionchatapp-match:0.0.1
```

```
kubectl kustomize ./k8s/ | kubectl apply -f -
```