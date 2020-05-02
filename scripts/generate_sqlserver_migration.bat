set ASPNETCORE_ENVIRONMENT=prod

dotnet tool install --global dotnet-ef

rmdir "./../src/Owlvey.Falcon.Repositories/Migrations" /s /q

del /f "./../src/Owlvey.Falcon.Api/FalconDb.db"

dotnet build ./../Owlvey.Falcon.sln -v:q

pushd "./../src/Owlvey.Falcon.Repositories"

echo dotnet ef migrations add initial create 

dotnet ef migrations add InitialCreate

popd

rmdir "./../infrastructure/relational/falcondb.sql" /s /q

echo build components 

pushd "./../src/Owlvey.Falcon.Repositories"

echo migrations 

dotnet ef migrations script -c FalconDbContext -o ../../infrastructure/relational/falcondb.sql --verbose

notepad "../../infrastructure/relational/falcondb.sql"

popd




