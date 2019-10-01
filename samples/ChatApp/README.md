docker build -t agones-udp-server-magiconionchatapp:0.1.0 -f ChatApp.Server/Dockerfile .
docker tag agones-udp-server-magiconionchatapp:0.1.0 guitarrapc/agones-udp-server-magiconionchatapp:0.1.0
docker push guitarrapc/agones-udp-server-magiconionchatapp:0.1.0
