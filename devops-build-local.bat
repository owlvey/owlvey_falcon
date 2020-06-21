dotnet clean ./Owlvey.Falcon.sln -v:q

pushd "./containers/api"

docker-compose build

popd

docker tag owlvey/api localhost:48700/owlvey/api
docker push localhost:48700/owlvey/api

pushd "./containers/relational"

docker-compose build

popd

docker tag owlvey/relational localhost:48700/owlvey/relational
docker push localhost:48700/owlvey/relational

