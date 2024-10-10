@echo off
SET /a dt=%DATE:-=% + 1010
SET yy=%dt:~2,2%
SET mm=%dt:~4,2%
SET dd=%dt:~6,2%
SET /a hh=%TIME:~0,2% + 10
SET min=%TIME:~3,2%

SET version=0.21%yy%.%mm%%dd%.%hh%%min%
SET appName=ZzzLab.Web

IF NOT "%2" == "" SET version=%2

echo ===============================
echo %version% 
echo ===============================

del *.bak

cd .\src

dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer                
dotnet add package Swashbuckle.AspNetCore                   
dotnet add package Swashbuckle.AspNetCore.Filters              
dotnet add package Swashbuckle.AspNetCore.Newtonsoft      
dotnet add package ZzzLab.Crypt

dotnet list package

dotnet clean
dotnet publish -p:Version=%version% -p:Configuration=Release
dotnet pack --output ..\..\..\Package -p:Version=%version% -p:Configuration=Release

IF NOT "%1" == "" (
    SET ApiToken=%1
    dotnet nuget push ..\..\..\Package\%appName%.%version%.nupkg --api-key %ApiToken% --source https://api.nuget.org/v3/index.json
    start chrome https://www.nuget.org/packages/%appName%/
)

cd ..

IF "%2" == "" timeout /t 20