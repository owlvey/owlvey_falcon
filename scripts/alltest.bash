# dotnet tool install --global coverlet.console
# https://github.com/danielpalme/ReportGenerator
# dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet build ./../src/Owlvey.Falcon.Core/Owlvey.Falcon.Core.csproj -v q
dotnet build ./../tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj

dotnet build ./../tests/Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj

dotnet build ./../tests/Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.IntegrationTests/Owlvey.Falcon.IntegrationTests.csproj