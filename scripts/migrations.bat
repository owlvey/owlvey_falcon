pushd "./../src/Owlvey.Falcon.API"
del /f FalconDb.db
popd
pushd "./../src/Owlvey.Falcon.Repositories"
rmdir Migrations /s /q
dotnet ef migrations add InitialCreate
popd