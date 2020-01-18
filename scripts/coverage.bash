# dotnet tool install --global coverlet.console
# https://github.com/danielpalme/ReportGenerator
# dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet build ./../src/Owlvey.Falcon.Core/Owlvey.Falcon.Core.csproj -v q
dotnet build ./../tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj  -v q
dotnet test ./../tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj
coverlet "./../tests/Owlvey.Falcon.UnitTests/bin/Debug/netcoreapp3.1/Owlvey.Falcon.UnitTests.dll" --target "dotnet" --targetargs "test ./../tests/Owlvey.Falcon.UnitTests/Owlvey.Falcon.UnitTests.csproj --no-build" --format opencover --output "./../tests/Owlvey.Falcon.UnitTests/BuildReports/"
reportgenerator "-reports:coverage.json" "-targetdir:.\BuildReports"
reportgenerator -reports:./../tests/Owlvey.Falcon.UnitTests/BuildReports/coverage.opencover.xml -reporttypes:HTML -targetdir:"./../tests/Owlvey.Falcon.UnitTests/BuildReports/report"
open ./../tests/Owlvey.Falcon.UnitTests/BuildReports/report/index.htm 