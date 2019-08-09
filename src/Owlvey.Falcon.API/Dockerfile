FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Owlvey.Falcon.API/Owlvey.Falcon.API.csproj", "src/Owlvey.Falcon.API/"]
COPY ["src/Owlvey.Falcon.Identity/Owlvey.Falcon.Identity.csproj", "src/Owlvey.Falcon.Identity/"]
COPY ["src/Owlvey.Falcon.Components/Owlvey.Falcon.Components.csproj", "src/Owlvey.Falcon.Components/"]
COPY ["src/Owlvey.Falcon.Core/Owlvey.Falcon.Core.csproj", "src/Owlvey.Falcon.Core/"]
COPY ["src/Owlvey.Falcon.Options/Owlvey.Falcon.Options.csproj", "src/Owlvey.Falcon.Options/"]
COPY ["src/Owlvey.Falcon.IoC/Owlvey.Falcon.IoC.csproj", "src/Owlvey.Falcon.IoC/"]

RUN dotnet restore "src/Owlvey.Falcon.API/Owlvey.Falcon.API.csproj"
COPY . .
WORKDIR "/src/src/Owlvey.Falcon.API"
RUN dotnet build "Owlvey.Falcon.API.csproj" -c Release -o /app

WORKDIR "/src/src/Owlvey.Falcon.API"
FROM build AS publish
RUN dotnet publish "Owlvey.Falcon.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]