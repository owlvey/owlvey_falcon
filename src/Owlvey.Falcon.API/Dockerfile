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
COPY ["tests/Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj", "tests/Owlvey.Falcon.ComponentsTests/"]
COPY ["tests/Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj", "tests/Owlvey.Falcon.IntegrationTests/"]
COPY ["tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj", "tests/Owlvey.Falcon.UnitTests/"]
COPY ["tests/Owlvey.Falcon.ManualTests/Owlvey.Falcon.ManualTests.csproj", "tests/Owlvey.Falcon.ManualTests/"]

RUN dotnet restore "src/Owlvey.Falcon.API/Owlvey.Falcon.API.csproj"
COPY . .
WORKDIR "/src/src/Owlvey.Falcon.API"
RUN dotnet build "Owlvey.Falcon.API.csproj" -c Release -o /app

WORKDIR "/src/tests/"
RUN dotnet test "Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj" --logger "trx;LogFileName=unittests.trx"
RUN dotnet test "Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj" --logger "trx;LogFileName=componentstests.trx"
RUN dotnet test "Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj" --logger "trx;LogFileName=integrationtests.trx"
RUN dotnet test "Owlvey.Falcon.ManualTests/Owlvey.Falcon.ManualTests.csproj" --logger "trx;LogFileName=manualtests.trx"

WORKDIR "/src/src/Owlvey.Falcon.API"
FROM build AS publish
RUN dotnet publish "Owlvey.Falcon.API.csproj" -c Release -o /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]