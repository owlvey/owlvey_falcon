pushd "./../src/Owlvey.Falcon.API"
del /f FalconDb.db
popd
pushd "./../src/Owlvey.Falcon.Components"
rmdir Migrations /s /q
dotnet ef migrations add InitialCreate
popd