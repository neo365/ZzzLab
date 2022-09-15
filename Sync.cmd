@echo off

echo == ZzzLab.Core Sync ===========================

if NOT EXIST .\ZzzLab.Core\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Core ZzzLab.Core
)

cd ZzzLab.Core
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.DBClient Sync ===========================
if NOT EXIST .\ZzzLab.DBClient\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.DBClient ZzzLab.DBClient
)

cd ZzzLab.DBClient
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.Models Sync ===========================
if NOT EXIST .\ZzzLab.Models\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Models ZzzLab.Models
)

cd ZzzLab.Models
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.Office Sync ===========================
if NOT EXIST .\ZzzLab.Office\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Office ZzzLab.Office
)

cd ZzzLab.Office
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.Scheduler Sync ===========================
if NOT EXIST .\ZzzLab.Scheduler\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Scheduler ZzzLab.Scheduler
)

cd ZzzLab.Scheduler
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.Desktop Sync ===========================
if NOT EXIST .\ZzzLab.Desktop\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Desktop ZzzLab.Desktop
)

cd ZzzLab.Desktop
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == ZzzLab.Web Sync ===========================
if NOT EXIST .\ZzzLab.Web\ (
	git clone --progress -v https://github.com/neo365/ZzzLab.Web ZzzLab.Web
)

cd ZzzLab.Web
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..


echo == End of Document ===========================
timeout /t 120