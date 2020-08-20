
dotnet tool install --global dotnet-ef

rm -rf ./../src/Owlvey.Falcon.Repositories/Migrations

rm ./../src/Owlvey.Falcon.Api/FalconDb.db

pushd ./../src/Owlvey.Falcon.Repositories

dotnet ef migrations add InitialCreate

popd