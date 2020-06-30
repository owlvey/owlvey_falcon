dotnet clean ./Owlvey.Falcon.sln -v:q

pushd "./containers/api"

docker-compose build

popd

docker tag registry/api localhost:48700/registry/api
docker push localhost:48700/registry/api

