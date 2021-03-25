dotnet build Owlvey.Falcon.sln -v:q                

pushd ./src/Owlvey.Falcon.Repositories    

rm -r ./Migrations    

export ASPNETCORE_ENVIRONMENT=prod

dotnet ef migrations add InitialCreate -c FalconDbContext

dotnet ef migrations script -c FalconDbContext --configuration Release -o ./../Owlvey.Falcon.Database/migration/V1__Initial_Setup.sql 

popd

