$ENV:ASPNETCORE_ENVIRONMENT="development"
# https://github.com/dotnet/aspnetcore/issues/8449
dotnet build ./../Owlvey.Falcon.sln -v q
dotnet run --project ./../src/Owlvey.Falcon.API/Owlvey.Falcon.API.csproj  --urls=http://0.0.0.0:50001/
