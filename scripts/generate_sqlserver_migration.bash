export ASPNETCORE_ENVIRONMENT=prod
dotnet build ./../Owlvey.Falcon.sln -v q
cd ./../src/Owlvey.Falcon.Components
dotnet ef migrations script -c FalconDbContext -o ../../artifactory/falcondb.sql --verbose
open  ../../artifactory/falcondb.sql
cd  ../../scripts

