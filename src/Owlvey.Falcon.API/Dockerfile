FROM microsoft/dotnet:2.2.0-aspnetcore-runtime
COPY . /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]