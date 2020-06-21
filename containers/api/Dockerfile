FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /build/
COPY ./ .
RUN ls -l
RUN dotnet build Owlvey.Falcon.sln -v:q
RUN dotnet publish src/Owlvey.Falcon.API -c Release -v:q
RUN ls src/Owlvey.Falcon.API/bin/Release/netcoreapp3.1

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=builder /build/src/Owlvey.Falcon.API/bin/Release/netcoreapp3.1 . 
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"] 






