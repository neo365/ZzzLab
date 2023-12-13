@echo off
SET /a dt=%DATE:-=% + 1010
SET yy=%dt:~2,2%
SET mm=%dt:~4,2%
SET dd=%dt:~6,2%
SET /a hh=%TIME:~0,2% + 10
SET min=%TIME:~3,2%

SET version=0.21%yy%.%mm%%dd%.%hh%%min%
SET appName=ZzzLab.Office

IF NOT "%1" == "" SET version=%1

echo ===============================
echo %version% 
echo ===============================

del *.bak

cd .\src

dotnet add package NPOI   
dotnet add package PdfSharpCore   
dotnet add package ZzzLab.Core
dotnet add package iTextSharp.LGPLv2.Core
dotnet add package PDFium.Windows   
dotnet add package PDFiumSharpV2   
dotnet add package System.Drawing.Common   
dotnet add package System.Memory   

dotnet list package

dotnet clean
dotnet publish -p:Version=%version% -p:Configuration=Release
dotnet pack --output ..\..\..\Package -p:Version=%version%

IF NOT "%2" == "" (
    SET ApiToken=%2
    dotnet nuget push ..\..\..\Package\%appName%.%version%.nupkg --api-key %ApiToken% --source https://api.nuget.org/v3/index.json
    start chrome https://www.nuget.org/packages/%appName%/
)

cd ..

IF "%1" == "" timeout /t 20