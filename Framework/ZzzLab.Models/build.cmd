@echo off
SET /a dt=%DATE:-=% + 1010
SET yy=%dt:~2,2%
SET mm=%dt:~4,2%
SET dd=%dt:~6,2%
SET /a hh=%TIME:~0,2% + 10
SET min=%TIME:~3,2%

SET version=0.21%yy%.%mm%%dd%.%hh%%min%

IF NOT "%1" == "" SET version=%1

echo ===============================
echo %version% 
echo ===============================

del *.bak

git pull --progress

cd .\src

dotnet list package
dotnet add package ZzzLab.Core
dotnet list package

cd ..
git commit -a -m "Auto Build: "%version% &
git push --progress

cd .\src

dotnet clean
dotnet publish -p:Version=%version%
dotnet pack --output ..\..\..\Package -p:Version=%version%
cd ..

dotnet nuget push ..\..\Package\ZzzLab.Models.%version%.nupkg --api-key oy2obbaz5hmwbmexdheqsju4x6crlfce5bfckjimj6csfu --source https://api.nuget.org/v3/index.json

start chrome https://www.nuget.org/packages/ZzzLab.Models/

IF "%1" == "" timeout /t 120