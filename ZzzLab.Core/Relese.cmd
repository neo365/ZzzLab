@echo off

SET yy=%DATE:~2,2%
SET /a mm=%DATE:~5,2% + 10
SET dd=%DATE:~8,2%
SET /a hh=%TIME:~0,2% + 10
SET min=%TIME:~3,2%

SET version=0.21%yy%.%mm%%dd%.%hh%%min%

del *.bak

if EXIST ..\package\ (
    del /Q ..\package\*.*
)


cd ZzzLab.Core
call .\build.cmd %version%
cd ..

cd ZzzLab.Models
call .\build.cmd %version%
cd ..

cd ZzzLab.DBClient
call .\build.cmd %version%
cd ..

cd ZzzLab.Scheduler
call .\build.cmd %version%
cd ..

cd ZzzLab.Desktop
call .\build.cmd %version%
cd ..

cd ZzzLab.Office
call .\build.cmd %version%
cd ..

cd ZzzLab.Web
call .\build.cmd %version%
cd ..

timeout /t 120
