# dotnet tool install --global coverlet.console
# https://github.com/danielpalme/ReportGenerator
# dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet build ./../tests/Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj
