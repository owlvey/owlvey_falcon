#!/bin/bash
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -Q "CREATE DATABASE [FalconDb]"
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -d FalconDb -i tmp/falcon/falcondb.sql