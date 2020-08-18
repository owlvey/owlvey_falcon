Push-Location "./../src/Owlvey.Falcon.API"

Remove-Item -LiteralPath "FalconDb.db" -Force 

Pop-Location


Push-Location "./../src/Owlvey.Falcon.Repositories"

Remove-Item -LiteralPath "Migrations" -Force -Recurse

dotnet ef migrations add InitialCreate

Pop-Location