@echo off


SET ApiToken=%1

SET /a dt=%DATE:-=% + 1010
SET yy=%dt:~2,2%
SET mm=%dt:~4,2%
SET dd=%dt:~6,2%
SET /a hh=%TIME:~0,2% + 10
SET min=%TIME:~3,2%

SET version=0.21%yy%.%mm%%dd%.%hh%%min%

IF NOT "%2" == "" SET version=%2


del *.bak

if EXIST ..\package\ (
    del /Q ..\package\*.*
)

git pull --progress

cd ZzzLab.Core
call .\build.cmd %ApiToken% %version% 
cd ..

cd ZzzLab.Crypt
call .\build.cmd %ApiToken% %version% 
cd ..

cd ZzzLab.DBClient
call .\build.cmd %ApiToken% %version%
cd ..

cd ZzzLab.Scheduler
call .\build.cmd %ApiToken% %version%
cd ..

cd ZzzLab.Desktop
call .\build.cmd %version% %ApiToken%
cd ..

cd ZzzLab.Office
call .\build.cmd %ApiToken% %version%
cd ..

cd ZzzLab.Web
call .\build.cmd %ApiToken% %version%
cd ..

git commit -a -m "Auto Build: "%version% &
git push --progress

timeout /t 20
