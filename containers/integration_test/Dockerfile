FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /build/
COPY ./ .
RUN ls -l
RUN apt-get -y update
RUN apt-get -y install dnsutils

RUN dotnet build tests/Owlvey.Falcon.IntegrationTests -v:q
RUN dotnet publish tests/Owlvey.Falcon.IntegrationTests -c Release -v:q
RUN ls tests/Owlvey.Falcon.IntegrationTests/bin/Release/netcoreapp3.1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 
WORKDIR /app
COPY --from=builder /build/tests/Owlvey.Falcon.IntegrationTests/bin/Release/netcoreapp3.1 . 
ENTRYPOINT ["dotnet", "test", "Owlvey.Falcon.IntegrationTests.dll"] 






