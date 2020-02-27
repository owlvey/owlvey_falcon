docker pull influxdb:1.7.10

docker run -d -p 8086:8086 -e INFLUXDB_GRAPHITE_ENABLED=true influxdb     

curl -i -XPOST http://localhost:8086/query --data-binary "q=CREATE DATABASE falcondb"

