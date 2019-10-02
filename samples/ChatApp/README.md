docker build -t agones-udp-server-magiconionchatapp:0.1.1 -f ChatApp.Server/Dockerfile .
docker tag agones-udp-server-magiconionchatapp:0.1.1 guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
docker push guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
