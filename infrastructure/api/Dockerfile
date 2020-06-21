FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /build/api/
COPY ./../ .
RUN 




FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY . /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]
