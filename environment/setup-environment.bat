docker ps
docker stop owlvey_db

timeout /t 10

docker run --name owlvey_db --rm --env ACCEPT_EULA=Y --env MSSQL_SA_PASSWORD=TheFalcon123 --env MSSQL_PID=Express  --env MSSQL_TCP_PORT=1433  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest

timeout /t 30

docker exec owlvey_db /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P TheFalcon123 -Q "create database FalconDb;"
docker exec owlvey_db /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P TheFalcon123 -Q "alter database FalconDb set auto_close off;" 