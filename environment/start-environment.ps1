pushd ./../src/Owlvey.Falcon.Database

flyway -configFiles="local.conf" info
flyway -configFiles="local.conf" migrate
flyway -configFiles="local.conf" info

popd

pushd ./../src/Owlvey.Falcon.API

dotnet build

Start-Process dotnet run

popd