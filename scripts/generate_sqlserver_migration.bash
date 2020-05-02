export ASPNETCORE_ENVIRONMENT=prod

dotnet tool install --global dotnet-ef

rm -rf ./../src/Owlvey.Falcon.Repositories/Migrations

rm ./../src/Owlvey.Falcon.Api/FalconDb.db

dotnet build ./../Owlvey.Falcon.sln -v:q

pushd ./../src/Owlvey.Falcon.Repositories

echo dotnet ef migrations add initial create 

dotnet ef migrations add InitialCreate

popd

rm ./../infrastructure/relational/falcondb.sql

echo build components 

pwd 

pushd ./../src/Owlvey.Falcon.Repositories

pwd

echo migrations 

dotnet ef migrations script -c FalconDbContext -o ../../infrastructure/relational/falcondb.sql --verbose

cat ../../infrastructure/relational/falcondb.sql

popd




