@echo off

ECHO Removing previous build...
dotnet clean --configuration Release --verbosity minimal

ECHO Restoring dependencies...
dotnet restore

ECHO Building...
dotnet build --no-restore --configuration Release

%SystemRoot%\explorer.exe "%~f1src\Osu.Music\bin\Release\netcoreapp3.1"