rm -rf ./../src/Owlvey.Falcon.Components/Migrations

rm ./../src/Owlvey.Falcon.Api/FalconDb.db

pushd ./../src/Owlvey.Falcon.Components

dotnet ef migrations add InitialCreate

popd