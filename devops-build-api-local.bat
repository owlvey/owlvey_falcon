dotnet clean ./Owlvey.Falcon.sln -v:q

pushd "./containers/api"

rem docker-compose build

popd

rem docker tag registry/api localhost:48700/registry/api
rem docker push localhost:48700/registry/api

pushd "./containers/integration_test"

docker-compose build

popd

docker tag registry/api-integration-test localhost:48700/registry/api-integration-test
docker push localhost:48700/registry/api-integration-test

