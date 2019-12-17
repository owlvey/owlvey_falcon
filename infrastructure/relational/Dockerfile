# https://www.sqlshack.com/creating-your-own-sql-server-docker-image/

FROM mcr.microsoft.com/mssql/server:2017-latest
    ENV ACCEPT_EULA=Y
    ENV MSSQL_SA_PASSWORD=TheFalcon123
    ENV MSSQL_PID=Express
    ENV MSSQL_TCP_PORT=1433 
    
    COPY falcondb.sql /dbbackups/
    WORKDIR /src
    RUN echo $MSSQL_SA_PASSWORD

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -Q"create database FalconDb;" 

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -Q"alter database FalconDb set auto_close off;" 

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -d Owlvey -i '/dbbackups/falcondb.sql'
    
    EXPOSE 1433/tcp
