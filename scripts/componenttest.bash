# dotnet tool install --global coverlet.console
# https://github.com/danielpalme/ReportGenerator
# dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet build ./../tests/Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.ComponentsTests/Owlvey.Falcon.ComponentsTests.csproj
