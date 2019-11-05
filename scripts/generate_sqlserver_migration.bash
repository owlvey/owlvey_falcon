export ASPNETCORE_ENVIRONMENT=prod

rm -rf ./../src/Owlvey.Falcon.Components/Migrations

rm ./../src/Owlvey.Falcon.Api/FalconDb.db

pushd ./../src/Owlvey.Falcon.Components

dotnet ef migrations add InitialCreate

popd

rm ./../artifactory/falcondb.sql
dotnet build ./../Owlvey.Falcon.sln -v q
cd ./../src/Owlvey.Falcon.Components
dotnet ef migrations script -c FalconDbContext -o ../../artifactory/falcondb.sql --verbose
cat  ../../artifactory/falcondb.sql
cd  ../../scripts



