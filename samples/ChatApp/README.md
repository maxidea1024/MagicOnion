docker build -t agones-magiconionchatapp:0.1.2 -f ChatApp.Server/Dockerfile .
docker tag agones-magiconionchatapp:0.1.2 guitarrapc/agones-magiconionchatapp:0.1.2
docker push guitarrapc/agones-magiconionchatapp:0.1.2

