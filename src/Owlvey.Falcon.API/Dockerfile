FROM microsoft/dotnet:3.1.0-aspnetcore-runtime
COPY . /app
RUN mkdir -p /app/data
RUN chmod o+x /app/data
COPY falcondb.sql /app/data
COPY falcondb.sh /app/data
RUN chmod +x /app/data/falcondb.sql
RUN chmod +x /app/data/falcondb.sh
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]