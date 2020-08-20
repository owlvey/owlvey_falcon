dotnet clean ./Owlvey.Falcon.sln -v:q

pushd "./containers/relational"

docker-compose build

popd

docker tag owlveydevcommit/relational localhost:48700/owlveydevcommit/relational
docker push localhost:48700/owlveydevcommit/relational

