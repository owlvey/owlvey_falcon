export ASPNETCORE_ENVIRONMENT=prod

dotnet tool install --global dotnet-ef

rm -rf ./../src/Owlvey.Falcon.Components/Migrations

rm ./../src/Owlvey.Falcon.Api/FalconDb.db

pushd ./../src/Owlvey.Falcon.Components

dotnet build ./../Owlvey.Falcon.sln -v q


echo dotnet ef migrations add initial create 

dotnet ef migrations add InitialCreate

popd

rm ./../artifactory/falcondb.sql

echo build components 

pwd 

cd ./../src/Owlvey.Falcon.Components

pwd

echo migrations 

dotnet ef migrations script -c FalconDbContext -o ../../artifactory/falcondb.sql --verbose

cat  ../../artifactory/falcondb.sql

cd  ../../scripts



