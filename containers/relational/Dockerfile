FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
    ENV ASPNETCORE_ENVIRONMENT=prod
    RUN dotnet tool install --global dotnet-ef         
    ENV PATH="$PATH:/root/.dotnet/tools"
    WORKDIR /build/    
    COPY ./ .    

    RUN dotnet build Owlvey.Falcon.sln -v:q                

    WORKDIR /build/src/Owlvey.Falcon.Repositories    
    
    RUN rm -r ./Migrations    

    RUN dotnet ef migrations add InitialCreate -c FalconDbContext

    RUN dotnet ef migrations script -c FalconDbContext -o /build/artifactory/falcondb.sql 

    RUN cat /build/artifactory/falcondb.sql

    RUN ls /build/artifactory


FROM mcr.microsoft.com/mssql/server:2017-latest
    ENV ACCEPT_EULA=Y
    ENV MSSQL_SA_PASSWORD=TheFalcon123
    ENV MSSQL_PID=Express
    ENV MSSQL_TCP_PORT=1433 

    COPY --from=builder /build/artifactory . 

    RUN ls -a
    
    WORKDIR /src

    RUN echo $MSSQL_SA_PASSWORD

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -Q"create database FalconDb;" 

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -Q"alter database FalconDb set auto_close off;" 

    RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S127.0.0.1 -Usa -PTheFalcon123  -d FalconDb -i '/falcondb.sql'
    
    EXPOSE 1433/tcp
